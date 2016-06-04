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
    internal interface ICommandMetadata : ILoaderMetadata
    {
        CommandMetadataCollection Commands { get; }

        object ExecuteCore(LoadingContextBase context, object parameter);
    }
}