using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CliAssignmentSymbolAttribute : Attribute
    {
        public CliAssignmentSymbolAttribute(string symbol)
        {
            Symbol = symbol;
        }

        public virtual string Symbol { get; }
    }
}