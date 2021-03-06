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
    public sealed class MethodCommandMetadata : CommandMetadata
    {
        private sealed class ExecutionContext : LoadingContextBase
        {
            public ExecutionContext(LoadingContextBase parentContext, LoaderMetadata metadata, LoaderSettings settings, IEnumerable<string> args, object target, object[] parameters)
                : base(parentContext, metadata, settings, args)
            {
                Target = target;
                Parameters = parameters;
            }

            public object Target { get; }

            public object[] Parameters { get; }
        }

        private static readonly CommandMetadataCollection _Commands = new CommandMetadataCollection(new CommandMetadata[0]);

        private ParameterMetadataCollection _Parameters;

        internal MethodCommandMetadata(MethodInfo method)
            : base(method)
        { 
        }

        protected override void LoadCore()
        {
            base.LoadCore();

            _Parameters = new ParameterMetadataCollection(Method.GetParameters().Select(_ => new ParameterMetadata(_)).ToArray());
        }

        public override string FullName => $"{Method.DeclaringType.FullName}.{Method.Name}";

        public MethodInfo Method => (MethodInfo)Member;

        public override CommandMetadataCollection Commands => _Commands;

        public ParameterMetadataCollection Parameters 
            => ((MethodCommandMetadata)EnsureLoaded())._Parameters;

        public override IReadOnlyList<CommandOptionMetadata> GetOptions()
            => Parameters;

        internal override LoadingContextBase CreateContextForCurrentObject(TypeMetadata metadata, LoaderSettings settings, IEnumerable<string> args, object target)
        {
            throw new NotSupportedException();
        }

        internal override LoadingContextBase CreateContextForDeclaringObject(CommandMetadata metadata, LoaderSettings settings, IEnumerable<string> args, LoadingContextBase parentContext)
            => new ExecutionContext(parentContext, metadata, settings, args, ((ObjectLoadingContext)parentContext).Target, CreateDefaultParameters());

        internal override object GetValue(LoadingContextBase context, CommandOptionMetadata metadata)
            => ((ExecutionContext)context).Parameters[((ParameterMetadata)metadata).Parameter.Position];

        internal override void SetValue(LoadingContextBase context, CommandOptionMetadata metadata, string value)
            => ((ExecutionContext)context).Parameters[((ParameterMetadata)metadata).Parameter.Position] = metadata.ConvertFrom(value);

        internal override object ExecuteCore(LoadingContextBase context, object parameter)
        {
            var ec = (ExecutionContext)context;

            // TODO:set parameter

            return Method.Invoke(ec.Target, ec.Parameters);
        }

        private object[] CreateDefaultParameters()
            => Method.GetParameters().Select(_ => _.ParameterType.IsValueType ? Activator.CreateInstance(_.ParameterType) : null).ToArray();
    }
}