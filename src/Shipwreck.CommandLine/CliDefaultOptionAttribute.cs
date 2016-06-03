using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CliDefaultOptionAttribute : Attribute
    {
        public CliDefaultOptionAttribute()
        {
            IsDefault = true;
        }

        public CliDefaultOptionAttribute(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public CliDefaultOptionAttribute(bool isDefault, int precedence)
        {
            IsDefault = isDefault;
            Precedence = precedence;
        }

        public virtual bool IsDefault { get; }

        public virtual int Precedence { get; }
    }
}