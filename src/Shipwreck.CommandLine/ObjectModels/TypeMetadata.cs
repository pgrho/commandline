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
    public class TypeMetadata : LoaderMetadata, ICommandMetadata
    {
        private static readonly Dictionary<Type, TypeMetadata> _Instances = new Dictionary<Type, TypeMetadata>();



        #region 遅延読み込みプロパティ

        private LoaderSettings _DefaultSettings;

        // TODO:値の設定
        private CommandMetadataCollection _Commands;

        private PropertyMetadataCollection _Properties;

        #endregion 遅延読み込みプロパティ

        internal TypeMetadata(Type type)
        {
            Type = type;
        }

        public override string FullName => Type.FullName;

        public Type Type { get; }

        #region 遅延読み込みプロパティ

        public LoaderSettings DefaultSettings
            => EnsureLoaded()._DefaultSettings;

        // TODO:値の設定
        public CommandMetadataCollection Commands
            => EnsureLoaded()._Commands;

        public PropertyMetadataCollection Properties
            => EnsureLoaded()._Properties;

        #endregion 遅延読み込みプロパティ

        #region Load

        /// <inheritdoc />
        protected new TypeMetadata EnsureLoaded()
           => (TypeMetadata)base.EnsureLoaded();

        /// <inheritdoc />
        protected override void LoadCore()
        {
            _DefaultSettings = new LoaderSettings(Type);

            var props = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var pms = props.Select(_ => new PropertyMetadata(_)).OrderBy(_ => _.Order).ToDictionary(_ => _.Property);

            var commandProps = props.Where(_ => _.GetCustomAttribute<CommandAttribute>() != null).ToArray();
            var methods = Type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.GetCustomAttribute<CommandAttribute>() != null);

            var cms = commandProps.Select(_ => (CommandMetadata)new PropertyCommandMetadata(pms[_])).Concat(methods.Select(_ => new MethodCommandMetadata(_))).OrderBy(_ => _.Order).ToArray();

            _Commands = new CommandMetadataCollection(cms);
            _Properties = new PropertyMetadataCollection(pms.Values.Where(_ => Array.IndexOf(commandProps, _.Property) < 0).ToArray());
        }

        #endregion Load

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

        public override IReadOnlyList<CommandOptionMetadata> GetOptions()
            => Properties;

        internal override LoadingContextBase CreateContextForCurrentObject(TypeMetadata metadata, LoaderSettings settings, IEnumerable<string> args, object target)
             => new ObjectLoadingContext(metadata, settings, args, target);

        internal override object GetValue(LoadingContextBase context, CommandOptionMetadata metadata)
            => ((PropertyMetadata)metadata).GetValue(((ObjectLoadingContext)context).Target);

        internal override void SetValue(LoadingContextBase context, CommandOptionMetadata metadata, string value)
            => ((PropertyMetadata)metadata).SetValue(((ObjectLoadingContext)context).Target, value);
        object ICommandMetadata.ExecuteCore(LoadingContextBase context, object parameter)
            => ((ICliCommand)((ObjectLoadingContext)context).Target).Execute(parameter);
    }
}