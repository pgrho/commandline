using Shipwreck.CommandLine.Markup;
using Shipwreck.CommandLine.Properties;
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
    public abstract class LoaderMetadata : MetadataBase, ILoaderMetadata
    {
        public abstract string FullName { get; }

        public abstract IReadOnlyList<CommandOptionMetadata> GetOptions();

        internal abstract LoadingContextBase CreateContextForCurrentObject(TypeMetadata metadata, LoaderSettings settings, IEnumerable<string> args, object target);

        internal abstract object GetValue(LoadingContextBase context, CommandOptionMetadata metadata);

        internal abstract void SetValue(LoadingContextBase context, CommandOptionMetadata metadata, string value);

        void ILoaderMetadata.LoadCore(LoadingContextBase context)
            => LoadCore(context);

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

                        LoadExplicitOption(context, path, value);
                    }
                    else
                    {
                        var path = a.Substring(si);

                        LoadExplicitOption(context, path, null);
                    }
                }
                else
                {
                    LoadAnonymousOptions(context, i);
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
                        LoadExplicitOption(context, prevPath, null);
                        prevPath = a.Substring(si);
                    }
                    else
                    {
                        LoadExplicitOption(context, prevPath, a);
                        prevPath = null;
                    }
                }
                else if (km.Success)
                {
                    prevPath = a.Substring(si);
                }
                else
                {
                    LoadAnonymousOptions(context, i);
                    return;
                }
            }

            if (prevPath != null)
            {
                LoadExplicitOption(context, prevPath, null);
            }
        }

        private void LoadAnonymousOptions(LoadingContextBase context, int startIndex)
        {
            var args = context.Args;
            var metadata = context.Metadata;
            var allOps = metadata.GetOptions();
            var rem = startIndex == 0 ? args : args.Skip(startIndex).ToArray();

            var ops = allOps.Where(_ => (context.CurrentOrder < 0 || _.Order >= context.CurrentOrder)
                                                && _.AllowAnonymous
                                                && !context.LoadedOptions.Contains(_))
                                        .Select((_, i) => new { Metadata = _, Index = i })
                                        .OrderBy(_ => _.Metadata.AnonymousPrecedence)
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
                    SetAnonymousOption(context, pm, rem[j++]);
                }

                SetAnonymousOption(context, last, string.Join(" ", rem.Skip(j)));
            }
            else
            {
                for (var j = 0; j < rem.Count; j++)
                {
                    SetAnonymousOption(context, ops[j], rem[j]);
                }
            }
        }

        private static void SetAnonymousOption(LoadingContextBase context, CommandOptionMetadata option, string value)
        {
            try
            {
                context.Metadata.SetValue(context, option, value);
            }
            catch (CommandLineParsingException ex)
            {
                ex.Value = value;
                throw;
            }
            catch (Exception ex)
            {
                var markup = MarkupDocument.Parse(
                                string.Format(
                                    SR.InvalidValueArg0ForOptionArg1Markup,
                                    MarkupRun.Escape(value),
                                    MarkupRun.Escape(option.Name)));
                markup.Freeze();

                throw new CommandLineParsingException(markup, ex)
                {
                    Value = value
                };
            }
        }

        private void LoadExplicitOption(LoadingContextBase context, string propertyPath, string value)
        {
            var paths = propertyPath.Split('.');

            var metadata = context.Metadata;
            var propertyMetadata = metadata
                                        .GetOptions()
                                        .FindByName(paths[0]);

            if (propertyMetadata == null)
            {
                throw new CommandLineParsingException(
                        string.Format(
                            "\"{0}\"に\"{1}\"に該当するオプションが存在しません。[{2}]のいずれかを指定してください。",
                            context.Metadata.FullName,
                            paths[0],
                            string.Join(", ", context.Metadata.GetOptions().Where(_ => !_.IsIgnored).Select(_ => _.Name))))
                {
                    Option = propertyPath,
                    Value = value
                };
            }

            if (context.CurrentOrder >= 0 && propertyMetadata.Order < context.CurrentOrder)
            {
                throw new CommandLineParsingException(
                        string.Format(
                            "\"{0}\"の\"{1}\"オプションをこの位置で指定することはできません。",
                            context.Metadata.FullName,
                            paths[0]))
                {
                    Option = propertyPath,
                    Value = value
                };
            }

            if (paths.Length == 1)
            {
                metadata.SetValue(context, propertyMetadata, value);
            }
            else
            {
                var v = metadata.GetValue(context, propertyMetadata);

                // TODO:複合パスが指定され、値がNULLだった場合の動作設定

                if (v == null)
                {
                    throw new CommandLineParsingException(
                            string.Format(
                                "\"{0}\"の\"{1}\"から始まる複合パスが指定されましたが、\"{1}\"の値はnullです。",
                                context.Metadata.FullName,
                                paths[0]))
                    {
                        Option = propertyPath,
                        Value = value
                    };
                }

                try
                {
                    LoadProperty(TypeMetadata.FromType(v.GetType()), v, new ArraySegment<string>(paths, 1, paths.Length - 1), value);
                }
                catch (CommandLineParsingException ex)
                {
                    ex.Option = propertyPath;
                    ex.Value = value;
                    throw;
                }
                catch (Exception ex)
                {
                    var markup = MarkupDocument.Parse(
                                    string.Format(
                                        SR.InvalidValueArg0ForOptionArg1Markup,
                                        MarkupRun.Escape(value),
                                        MarkupRun.Escape(propertyPath)));
                    markup.Freeze();

                    throw new CommandLineParsingException(markup, ex)
                    {
                        Option = propertyPath,
                        Value = value
                    };
                }
            }

            context.CurrentOrder = propertyMetadata.Order;
            context.LoadedOptions.Add(propertyMetadata);
        }

        private void LoadProperty(TypeMetadata metadata, object target, ArraySegment<string> propertyPath, string value)
        {
            var p = metadata.Properties[propertyPath.First()];

            if (p == null)
            {
                throw new CommandLineParsingException(
                        string.Format(
                            "\"{0}\"に\"{1}\"に該当するオプションが存在しません。[{2}]のいずれかを指定してください。",
                            metadata.FullName,
                            p,
                            string.Join(", ", metadata.GetOptions().Where(_ => !_.IsIgnored).Select(_ => _.Name))));
            }

            if (propertyPath.Count == 1)
            {
                p.SetValue(target, value);
            }
            else
            {
                var v = p.GetValue(target);

                // TODO:複合パスが指定され、値がNULLだった場合の動作設定

                if (v == null)
                {
                    throw new CommandLineParsingException(
                            string.Format(
                                "\"{0}\"の\"{1}\"から始まる複合パスが指定されましたが、\"{1}\"の値はnullです。",
                                target.GetType().FullName,
                                p));
                }

                LoadProperty(TypeMetadata.FromType(v.GetType()), v, new ArraySegment<string>(propertyPath.Array, propertyPath.Offset + 1, propertyPath.Count - 1), value);
            }
        }
    }
}