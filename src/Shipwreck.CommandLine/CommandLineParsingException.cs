using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public class CommandLineParsingException : Exception
    {
        public CommandLineParsingException(string message)
            : base(message)
        {
        }

        public CommandLineParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public string Option { get; internal set; }

        public string Value { get; internal set; }
    }
}
