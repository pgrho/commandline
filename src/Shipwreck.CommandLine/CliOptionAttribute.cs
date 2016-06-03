using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CliOptionAttribute : Attribute
    {
        public CliOptionAttribute(string name)
        {
            Name = name;
        }

        public virtual string Name { get; }
    }
}