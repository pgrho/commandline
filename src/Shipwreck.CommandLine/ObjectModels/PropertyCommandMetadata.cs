using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public sealed class PropertyCommandMetadata : CommandMetadata
    {
        private readonly PropertyMetadata _Property;

        internal PropertyCommandMetadata(PropertyMetadata property)
            : base(property.Property)
        {
            _Property = property;
        }

        public override CommandMetadataCollection Commands => _Property.TypeMetadata.Commands;

        internal override LoadingContextBase CreateContextForCurrentObject(TypeMetadata metadata, LoaderSettings settings, IEnumerable<string> args, object target)
            => new ObjectLoadingContext(metadata, settings, args, target);

        internal override LoadingContextBase CreateContextForDeclaringObject(CommandMetadata metadata, LoaderSettings settings, IEnumerable<string> args, LoadingContextBase parentContext)
            => new ObjectLoadingContext(parentContext, metadata, settings, args, _Property.GetValue(((ObjectLoadingContext)parentContext).Target));

        internal override IReadOnlyList<CommandOptionMetadata> GetOptions()
            => _Property.TypeMetadata.Options;

        internal override object GetValue(LoadingContextBase context, CommandOptionMetadata metadata)
            => _Property.TypeMetadata.GetValue(context, metadata);

        internal override void SetValue(LoadingContextBase context, CommandOptionMetadata metadata, string value)
            => _Property.TypeMetadata.SetValue(context, metadata, value);

        internal override object ExecuteCore(LoadingContextBase context, object parameter)
        {
            var t = ((ObjectLoadingContext)context).Target;

            return ((ICliCommand)t).Execute(parameter);
        }
    }
}