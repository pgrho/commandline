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
            : this(MarkupDocument.FromText(message))
        {
        }

        public CommandLineParsingException(MarkupDocument markup)
            : base(null)
        {
            Markup = markup;
        }

        public CommandLineParsingException(string message, Exception innerException)
            : this(MarkupDocument.FromText(message), innerException)
        {
        }

        public CommandLineParsingException(MarkupDocument markup, Exception innerException)
            : base(null, innerException)
        {
            Markup = markup;
        }

        public MarkupDocument Markup { get; }

        public override string Message
            => Markup.ToString();

        public string Option { get; internal set; }

        public string Value { get; internal set; }
    }
}