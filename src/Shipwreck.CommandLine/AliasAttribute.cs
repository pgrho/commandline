using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
    public class CommandLineAliasAttributeAttribute : Attribute
    {
        public CommandLineAliasAttributeAttribute(string alias)
        {
            Alias = alias;
        }

        public virtual string Alias { get; }
    }
}