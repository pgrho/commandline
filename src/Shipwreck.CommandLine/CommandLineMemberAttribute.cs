using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public abstract class CommandLineMemberAttribute : Attribute
    {
        public string Name { get; set; }

        public bool IsIgnored { get; set; }
    }

}