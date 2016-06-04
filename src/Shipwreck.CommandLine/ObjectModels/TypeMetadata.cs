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
    public sealed class TypeMetadata : LoaderMetadata
    {
        private static readonly Dictionary<Type, TypeMetadata> _Instances = new Dictionary<Type, TypeMetadata>();

        private TypeMetadata(Type type)
        {
            Type = type;
            DefaultSettings = new LoaderSettings(type);
            Options = new PropertyMetadataCollection(type.GetProperties().Select(_ => new PropertyMetadata(_)).OrderBy(_ => _.Order).ToArray());
        }

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
                    _Instances[type] = r = new TypeMetadata(type);
                }
            }
            return r;
        }

        internal override IReadOnlyList<CommandOptionMetadata> GetOptions()
            => Options;

        internal override LoadingContextBase CreateContextForCurrentObject(TypeMetadata metadata, LoaderSettings settings, IEnumerable<string> args, object target)
             => new ObjectLoadingContext(metadata, settings, args, target);

        internal override object GetValue(LoadingContextBase context, CommandOptionMetadata metadata)
            => ((PropertyMetadata)metadata).GetValue(((ObjectLoadingContext)context).Target);

        internal override void SetValue(LoadingContextBase context, CommandOptionMetadata metadata, string value)
            => ((PropertyMetadata)metadata).SetValue(((ObjectLoadingContext)context).Target, value);
    }
}