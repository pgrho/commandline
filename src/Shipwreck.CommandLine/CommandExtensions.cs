using Shipwreck.CommandLine.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public static class CommandExtensions
    {
        public static void Load<TParameter, TResult>(this ICliCommand<TParameter, TResult> command, IEnumerable<string> args, TypeMetadata metadata = null, CliLoadingSettings settings = null)
        {
            metadata = metadata ?? TypeMetadata.FromType(command.GetType());
            settings = settings ?? metadata.DefaultSettings;

            var context = metadata.CreateContextForCurrentObject(metadata, settings, args, command);

            metadata.LoadCore(context);
        }

        private static TResult ExecuteCore<TParameter, TResult>(LoadingContextBase context, TParameter parameter)
        {
            var metadata = (CommandMetadata)context.Metadata;
            var f = context.Args.FirstOrDefault();

            if (f != null)
            {
                // TODO:indexer
                var cmd = metadata.Commands.FirstOrDefault(_ => _.NamesPattern.IsMatch(f));

                if (cmd != null)
                {
                    var nc = cmd.CreateContextForDeclaringObject(cmd, context.Settings, context.Args.Skip(1), context);

                    return ExecuteCore<TParameter, TResult>(nc, parameter);
                }
            }

            metadata.LoadCore(context);
            return (TResult)metadata.ExecuteCore(context, parameter);
        }

        public static TResult Execute<TParameter, TResult>(this ICliCommand<TParameter, TResult> command, TParameter parameter, IEnumerable<string> args, TypeMetadata metadata = null, CliLoadingSettings settings = null)
        {
            metadata = metadata ?? TypeMetadata.FromType(command.GetType());
            settings = settings ?? metadata.DefaultSettings;

            var context = metadata.CreateContextForCurrentObject(metadata, settings, args, command);

            return ExecuteCore<TParameter, TResult>(context, parameter);
        }
    }
}