using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ArgumentStyleAttribute : Attribute
    {
        public ArgumentStyleAttribute(ArgumentStyle argumentStyle)
        {
            ArgumentStyle = argumentStyle;
        }

        public virtual ArgumentStyle ArgumentStyle { get; }
    }
}