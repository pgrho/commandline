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
    public abstract class CommandMetadata : LoaderMetadata, ICommandMetadata, INamedMetadata
    {
        internal CommandMetadata(MemberInfo member)
        {
            _NameStore = new MemberNameStore(member.Name, member, true);

            var attr = member.GetCustomAttribute<OptionAttribute>();

            IsIgnored = member.GetCustomAttribute<IgnoreAttribute>()?.IsIgnored
                            ?? attr?.IsIgnored
                            ?? false;

            Order = member.GetCustomAttribute<OptionOrderAttribute>()?.Order
                    ?? attr?.Order
                    ?? -1;
        }

        private readonly MemberNameStore _NameStore;

        public string Name => _NameStore.Name;

        public ReadOnlyCollection<string> Names => _NameStore.Names;

        public Regex NamesPattern => _NameStore.NamesPattern;
        public bool IsIgnored { get; }

        public int Order { get; }

        public abstract CommandMetadataCollection Commands { get; }

        internal abstract LoadingContextBase CreateContextForDeclaringObject(CommandMetadata metadata, LoaderSettings settings, IEnumerable<string> args, LoadingContextBase parentContext);

        object ICommandMetadata.ExecuteCore(LoadingContextBase context, object parameter)
            => ExecuteCore(context, parameter);

        internal abstract object ExecuteCore(LoadingContextBase context, object parameter);
    }
}