using Shipwreck.CommandLine.Markup;
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
            : this(new HelpMarkup().Text(message).Freeze())
        {
        }

        public CommandLineParsingException(HelpMarkup markup)
            : base(null)
        {
            Markup = markup;
        }

        public CommandLineParsingException(string message, Exception innerException)
            : this(new HelpMarkup().Text(message).Freeze(), innerException)
        {
        }

        public CommandLineParsingException(HelpMarkup markup, Exception innerException)
            : base(null, innerException)
        {
            Markup = markup;
        }

        public HelpMarkup Markup { get; }

        public override string Message
            => Markup.ToString();

        public string Option { get; internal set; }

        public string Value { get; internal set; }
    }
}