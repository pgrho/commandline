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
    internal abstract class LoadingContextBase
    {
        private HashSet<CommandOptionMetadata> _LoadedOptions;

        internal LoadingContextBase(LoaderMetadata metadata, LoaderSettings settings, IEnumerable<string> args)
            : this(null, metadata, settings, args)
        {
        }
        internal LoadingContextBase(LoadingContextBase parentContext, LoaderMetadata metadata, LoaderSettings settings, IEnumerable<string> args)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Metadata = metadata;
            Settings = settings;
            Args = args as IReadOnlyList<string> ?? args.ToArray();
            CurrentOrder = -1;
        }

        public LoadingContextBase ParentContext { get; }

        public LoaderMetadata Metadata { get; }

        public LoaderSettings Settings { get; }

        public IReadOnlyList<string> Args { get; }
        public int CurrentOrder { get; set; }

        public HashSet<CommandOptionMetadata> LoadedOptions
            => _LoadedOptions ?? (_LoadedOptions = new HashSet<CommandOptionMetadata>());
    }
}