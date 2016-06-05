using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class HelpText : HelpTextElement
    {
        private readonly string _Value;

        public HelpText(string value)
        {
            _Value = value;
        }

        public override void Write(StringBuilder builder)
            => builder.Append(_Value);

        public override void Write(HelpWriter writer)
            => writer.Write(_Value);
    }
}