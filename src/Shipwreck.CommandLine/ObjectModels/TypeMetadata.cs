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
    public class TypeMetadata : LoaderMetadata
    {
        private static readonly Dictionary<Type, TypeMetadata> _Instances = new Dictionary<Type, TypeMetadata>();

        internal TypeMetadata(Type type)
        {
            Type = type;
            DefaultSettings = new LoaderSettings(type);

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var pms = props.Select(_ => new PropertyMetadata(_)).OrderBy(_ => _.Order).ToDictionary(_ => _.Property);

            var commandProps = props.Where(_ => _.GetCustomAttribute<CommandAttribute>() != null).ToArray();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.GetCustomAttribute<CommandAttribute>() != null);

            var cms = commandProps.Select(_ => (CommandMetadata)new PropertyCommandMetadata(pms[_])).Concat(methods.Select(_ => new MethodCommandMetadata(_))).OrderBy(_ => _.Order).ToArray();

            Commands = new CommandMetadataCollection(cms);
            Options = new PropertyMetadataCollection(pms.Values.Where(_ => Array.IndexOf(commandProps, _.Property) < 0).ToArray());
        }


        public override string FullName => Type.FullName;

        public Type Type { get; }

        public LoaderSettings DefaultSettings { get; }

        // TODO:値の設定
        public CommandMetadataCollection Commands { get; }

        public PropertyMetadataCollection Options { get; }

        public static TypeMetadata FromType(Type type)
        {
            TypeMetadata r;
            lock (_Instances)
            {
                if (!_Instances.TryGetValue(type, out r))
                {
                    _Instances[type] = r = typeof(ICliCommand).IsAssignableFrom(type) ? new CommandTypeMetadata(type)
                                            : new TypeMetadata(type);
                }
            }
            return r;
        }

        public override IReadOnlyList<CommandOptionMetadata> GetOptions()
            => Options;

        internal override LoadingContextBase CreateContextForCurrentObject(TypeMetadata metadata, LoaderSettings settings, IEnumerable<string> args, object target)
             => new ObjectLoadingContext(metadata, settings, args, target);

        internal override object GetValue(LoadingContextBase context, CommandOptionMetadata metadata)
            => ((PropertyMetadata)metadata).GetValue(((ObjectLoadingContext)context).Target);

        internal override void SetValue(LoadingContextBase context, CommandOptionMetadata metadata, string value)
            => ((PropertyMetadata)metadata).SetValue(((ObjectLoadingContext)context).Target, value);
    }
}