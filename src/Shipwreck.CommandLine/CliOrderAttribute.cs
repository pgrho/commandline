using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CliOrderAttribute : Attribute
    {
        public CliOrderAttribute(int order)
        {
            Order = order;
        }

        public virtual int Order { get; }
    }
}