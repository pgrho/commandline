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
    public static class CliOptionLoader
    {
        public static void Load(object target, IEnumerable<string> args)
        {
            var md = CliTypeMetadata.FromType(target.GetType());

            Load(md, target, args);
        }

        public static void Load(CliTypeMetadata metadata, object target, IEnumerable<string> args)
            => Load(metadata, metadata.DefaultSettings, target, args);

        public static void Load(CliTypeMetadata metadata, CliLoadingSettings settings, object target, IEnumerable<string> args)
        {
            var ci = -1;
            foreach (var a in args)
            {
                var km = settings.KeyPattern.Match(a);
                if (km.Success)
                {
                    var si = km.Index + km.Length;
                    var am = settings.AssignmentPattern.Match(a, si);

                    if (am.Success)
                    {
                        var path = a.Substring(si, am.Index - si);

                        var value = a.Substring(am.Index + am.Length);

                        LoadProperty(metadata, settings, ref ci, target, path, value);
                    }
                    else
                    {
                        var path = a.Substring(si);

                        LoadProperty(metadata, settings, ref ci, target, path, null);
                    }
                }
                else
                {
                }
            }

            //if (settings.HasIndependentValue)
            //{
            //}
            //else
            //{
        }

        private static void LoadProperty(CliTypeMetadata metadata, CliLoadingSettings settings, ref int propertyIndex, object target, string propertyPath, string value)
        {
            var pns = propertyPath.Split('.');

            var fp = metadata.Options[pns[0]];

            if (fp == null)
            {
                // TODO:
                throw new Exception();
            }

            if (propertyIndex >= 0 && fp.Index < propertyIndex)
            {
                // TODO:
                throw new Exception();
            }
            propertyIndex = fp.Index;

            if (pns.Length == 1)
            {
                fp.SetValue(target, value);
            }
            else
            {
                var v = fp.GetValue(target);
                LoadProperty(CliTypeMetadata.FromType(v.GetType()), settings, v, pns.Skip(1), value);
            }
        }

        private static void LoadProperty(CliTypeMetadata metadata, CliLoadingSettings settings, object target, IEnumerable<string> propertyPath, string value)
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
                LoadProperty(CliTypeMetadata.FromType(v.GetType()), settings, v, pns.Skip(1), value);
            }
        }
    }
}