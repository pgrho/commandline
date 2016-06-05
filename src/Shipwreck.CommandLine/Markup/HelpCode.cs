using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class HelpCode : HelpTextElement
    {
        private readonly string _Value;

        public HelpCode(string value)
        {
            _Value = value;
        }

        public override void Write(StringBuilder builder)
            => builder.Append(_Value);

        public override void Write(HelpWriter writer)
            => writer.WriteCode(_Value);
    }
}
