using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public class CliOptionLoader
    {
        private int _CurrentOrder;
        private HashSet<CliOptionMetadata> _LoadedOptions;

        protected internal CliOptionLoader(CliTypeMetadata metadata, CliLoadingSettings settings, object target, IEnumerable<string> args)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Metadata = metadata;
            Settings = settings;
            Target = target;
            Args = args as IReadOnlyList<string> ?? args.ToArray();
            _CurrentOrder = -1;
        }

        protected CliTypeMetadata Metadata { get; }

        protected CliLoadingSettings Settings { get; }

        protected object Target { get; }

        protected IReadOnlyList<string> Args { get; }

        #region Load

        public static void Load(object target, IEnumerable<string> args)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            var md = CliTypeMetadata.FromType(target.GetType());

            Load(md, target, args);
        }

        public static void Load(CliTypeMetadata metadata, object target, IEnumerable<string> args)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            Load(metadata, metadata.DefaultSettings, target, args);
        }

        public static void Load(CliTypeMetadata metadata, CliLoadingSettings settings, object target, IEnumerable<string> args)
        {
            settings.CreateLoader(metadata, settings, target, args).LoadCore();
        }

        #endregion Load

        private void LoadCore()
        {
            for (var i = 0; i < Args.Count; i++)
            {
                var a = Args[i];

                var km = Settings.KeyPattern.Match(a);
                if (km.Success)
                {
                    var si = km.Index + km.Length;
                    var am = Settings.AssignmentPattern.Match(a, si);

                    if (am.Success)
                    {
                        var path = a.Substring(si, am.Index - si);

                        var value = a.Substring(am.Index + am.Length);

                        LoadRootProperty(path, value);
                    }
                    else
                    {
                        var path = a.Substring(si);

                        LoadRootProperty(path, null);
                    }
                }
                else
                {
                    LoadDefaultParameters(i);
                    return;
                }
            }
        }

        private void LoadDefaultParameters(int startIndex)
        {
            var rem = startIndex == 0 ? Args : Args.Skip(startIndex).ToArray();

            var ops = Metadata.Options.Where(_ => (_CurrentOrder < 0 || _.Order >= _CurrentOrder)
                                                && _.IsDefault
                                                && _LoadedOptions?.Contains(_) != true)
                                        .Select((_, i) => new { Metadata = _, Index = i })
                                        .OrderBy(_ => _.Metadata.Precedence)
                                        .Take(rem.Count)
                                        .OrderBy(_ => _.Index)
                                        .Select(_ => _.Metadata)
                                        .ToArray();

            if (rem.Count > ops.Length)
            {
                var last = Metadata.Options.Last(_ => Array.IndexOf(ops, _) >= 0);

                var j = 0;
                foreach (var pm in ops)
                {
                    if (pm == last)
                    {
                        continue;
                    }
                    pm.SetValue(Target, rem[j++]);
                }

                last.SetValue(Target, string.Join(" ", rem.Skip(j)));
            }
            else
            {
                for (var j = 0; j < rem.Count; j++)
                {
                    ops[j].SetValue(Target, rem[j]);
                }
            }
        }

        private void LoadRootProperty(string propertyPath, string value)
        {
            var pns = propertyPath.Split('.');

            var propertyMetadata = Metadata.Options[pns[0]];

            if (propertyMetadata == null)
            {
                throw new KeyNotFoundException();
            }

            if (_CurrentOrder >= 0 && propertyMetadata.Order < _CurrentOrder)
            {
                // TODO:
                throw new Exception();
            }

            if (pns.Length == 1)
            {
                propertyMetadata.SetValue(Target, value);
            }
            else
            {
                var v = propertyMetadata.GetValue(Target);
                LoadProperty(CliTypeMetadata.FromType(v.GetType()), v, pns.Skip(1), value);
            }
            _CurrentOrder = propertyMetadata.Order;
            (_LoadedOptions ?? (_LoadedOptions = new HashSet<CliOptionMetadata>())).Add(propertyMetadata);
        }

        private void LoadProperty(CliTypeMetadata metadata, object target, IEnumerable<string> propertyPath, string value)
        {
            var pns = propertyPath as IReadOnlyList<string> ?? propertyPath.ToArray();
            var p = metadata.Options[pns.First()];

            if (p == null)
            {
                // TODO:
                throw new Exception();
            }

            if (pns.Count == 1)
            {
                p.SetValue(target, value);
            }
            else
            {
                var v = p.GetValue(target);
                LoadProperty(CliTypeMetadata.FromType(v.GetType()), v, pns.Skip(1), value);
            }
        }
    }
}