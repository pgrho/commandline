using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CliArgumentStyleAttribute : Attribute
    {
        public CliArgumentStyleAttribute(CliArgumentStyle argumentStyle)
        {
            ArgumentStyle = argumentStyle;
        }

        public virtual CliArgumentStyle ArgumentStyle { get; }
    }
}