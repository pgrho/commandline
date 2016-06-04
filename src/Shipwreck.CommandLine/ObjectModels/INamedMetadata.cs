using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    internal interface INamedMetadata
    {
        string Name { get; }

        ReadOnlyCollection<string> Names { get; }
        Regex NamesPattern { get; }

        bool IsIgnored { get; }
    }
}