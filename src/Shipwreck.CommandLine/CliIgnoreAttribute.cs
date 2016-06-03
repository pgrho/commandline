using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CliIgnoreAttribute : Attribute
    {
        public CliIgnoreAttribute()
            : this(true)
        { }

        public CliIgnoreAttribute(bool isIgnored)
        {
            IsIgnored = true;
        }

        public virtual bool IsIgnored { get; }
    }
}