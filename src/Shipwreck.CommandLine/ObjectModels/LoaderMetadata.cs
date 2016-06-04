using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public abstract class LoaderMetadata
    {
        internal abstract IReadOnlyList<CommandOptionMetadata> GetOptions();

        internal abstract LoadingContextBase CreateContextForCurrentObject(TypeMetadata metadata, CliLoadingSettings settings, IEnumerable<string> args, object target);

        internal abstract object GetValue(LoadingContextBase context, CommandOptionMetadata metadata);

        internal abstract void SetValue(LoadingContextBase context, CommandOptionMetadata metadata, string value);


        internal void LoadCore(LoadingContextBase context)
        {
            switch (context.Settings.ArgumentStyle)
            {
                case ArgumentStyle.Combined:
                    LoadCombined(context);
                    break;
                case ArgumentStyle.Separated:
                    LoadSeparated(context);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        private void LoadCombined(LoadingContextBase context)
        {
            var settings = context.Settings;
            var args = context.Args;
            for (var i = 0; i < args.Count; i++)
            {
                var a = args[i];

                var km = settings.KeyPattern.Match(a);
                if (km.Success)
                {
                    var si = km.Index + km.Length;
                    var am = settings.AssignmentPattern.Match(a, si);

                    if (am.Success)
                    {
                        var path = a.Substring(si, am.Index - si);

                        var value = a.Substring(am.Index + am.Length);

                        LoadRootProperty(context, path, value);
                    }
                    else
                    {
                        var path = a.Substring(si);

                        LoadRootProperty(context, path, null);
                    }
                }
                else
                {
                    LoadDefaultParameters(context, i);
                    return;
                }
            }
        }

        private void LoadSeparated(LoadingContextBase context)
        {
            var settings = context.Settings;
            var args = context.Args;
            string prevPath = null;
            for (var i = 0; i < args.Count; i++)
            {
                var a = args[i];
                var km = settings.KeyPattern.Match(a);
                var si = km.Success ? km.Index + km.Length : 0;
                if (prevPath != null)
                {
                    if (km.Success)
                    {
                        LoadRootProperty(context, prevPath, null);
                        prevPath = a.Substring(si);
                    }
                    else
                    {
                        LoadRootProperty(context, prevPath, a);
                        prevPath = null;
                    }
                }
                else if (km.Success)
                {
                    prevPath = a.Substring(si);
                }
                else
                {
                    LoadDefaultParameters(context, i);
                    return;
                }
            }

            if (prevPath != null)
            {
                LoadRootProperty(context, prevPath, null);
            }
        }

        private void LoadDefaultParameters(LoadingContextBase context, int startIndex)
        {
            var args = context.Args;
            var metadata = context.Metadata;
            var allOps = metadata.GetOptions();
            var rem = startIndex == 0 ? args : args.Skip(startIndex).ToArray();

            var ops = allOps.Where(_ => (context.CurrentOrder < 0 || _.Order >= context.CurrentOrder)
                                                && _.IsDefault
                                                && !context.LoadedOptions.Contains(_))
                                        .Select((_, i) => new { Metadata = _, Index = i })
                                        .OrderBy(_ => _.Metadata.Precedence)
                                        .Take(rem.Count)
                                        .OrderBy(_ => _.Index)
                                        .Select(_ => _.Metadata)
                                        .ToArray();

            if (rem.Count > ops.Length)
            {
                var last = allOps.Last(_ => Array.IndexOf(ops, _) >= 0);

                var j = 0;
                foreach (var pm in ops)
                {
                    if (pm == last)
                    {
                        continue;
                    }
                    metadata.SetValue(context, pm, rem[j++]);
                }

                metadata.SetValue(context, last, string.Join(" ", rem.Skip(j)));
            }
            else
            {
                for (var j = 0; j < rem.Count; j++)
                {
                    metadata.SetValue(context, ops[j], rem[j]);
                }
            }
        }

        private void LoadRootProperty(LoadingContextBase context, string propertyPath, string value)
        {
            var pns = propertyPath.Split('.');

            var metadata = context.Metadata;
            var propertyMetadata = metadata
                                        .GetOptions()
                                        .FindByName(pns[0]);

            if (propertyMetadata == null)
            {
                throw new KeyNotFoundException();
            }

            if (context.CurrentOrder >= 0 && propertyMetadata.Order < context.CurrentOrder)
            {
                // TODO:
                throw new Exception();
            }

            if (pns.Length == 1)
            {
                metadata.SetValue(context, propertyMetadata, value);
            }
            else
            {
                var v = metadata.GetValue(context, propertyMetadata);
                LoadProperty(TypeMetadata.FromType(v.GetType()), v, pns.Skip(1), value);
            }
            context.CurrentOrder = propertyMetadata.Order;
            context.LoadedOptions.Add(propertyMetadata);
        }

        private void LoadProperty(TypeMetadata metadata, object target, IEnumerable<string> propertyPath, string value)
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
                LoadProperty(TypeMetadata.FromType(v.GetType()), v, pns.Skip(1), value);
            }
        }
    }
}