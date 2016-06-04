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
    internal sealed class CommandTypeMetadata : TypeMetadata, ICommandMetadata
    {
        internal CommandTypeMetadata(Type type)
            : base(type)
        {
        }

        object ICommandMetadata.ExecuteCore(LoadingContextBase context, object parameter)
            => ((ICliCommand)((ObjectLoadingContext)context).Target).Execute(parameter);
    }
}