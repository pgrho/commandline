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
    internal sealed class ObjectLoadingContext : LoadingContextBase
    {
        public ObjectLoadingContext(LoaderMetadata metadata, CliLoadingSettings settings, IEnumerable<string> args, object target)
            : base(metadata, settings, args)
        {
            Target = target;
        }
        public ObjectLoadingContext(LoadingContextBase parentContext, LoaderMetadata metadata, CliLoadingSettings settings, IEnumerable<string> args, object target)
            : base(parentContext, metadata, settings, args)
        {
            Target = target;
        }

        public object Target { get; }
    }
}