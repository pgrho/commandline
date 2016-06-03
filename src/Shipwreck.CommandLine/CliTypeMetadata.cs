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
    public sealed class CliTypeMetadata
    {
        private static readonly Dictionary<Type, CliTypeMetadata> _Instances = new Dictionary<Type, CliTypeMetadata>();

        private CliTypeMetadata(CliLoadingSettings defaultSettings, IEnumerable<CliOptionMetadata> options)
        {
            DefaultSettings = defaultSettings;
            Options = new CliOptionMetadataCollection(options.ToArray());
        }

        public CliLoadingSettings DefaultSettings { get; }

        public CliOptionMetadataCollection Options { get; }

        private static CliTypeMetadata CreateMetadata(Type type)
        {
            var style = type.GetCustomAttribute<CliArgumentStyleAttribute>()?.ArgumentStyle ?? CliArgumentStyle.Combined;
            var ks = type.GetCustomAttributes<CliKeySymbolAttribute>().Select(_ => _.Symbol).ToArray();

            var ass = type.GetCustomAttributes<CliAssignmentSymbolAttribute>().Select(_ => _.Symbol).ToArray();

            var ds = new CliLoadingSettings(style, ks.Any() ? ks : null, ass.Any() ? ass : null);

            var ps = type.GetProperties().Select(_ => CliOptionMetadata.FromPropertyInfo(_)).OrderBy(_ => _.Order);

            return new CliTypeMetadata(ds, ps);
        }

        public static CliTypeMetadata FromType(Type type)
        {
            CliTypeMetadata r;
            lock (_Instances)
            {
                if (!_Instances.TryGetValue(type, out r))
                {
                    _Instances[type] = r = CreateMetadata(type);
                }
            }
            return r;
        }
    }
}