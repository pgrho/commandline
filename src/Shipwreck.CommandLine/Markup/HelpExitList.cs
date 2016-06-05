using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class HelpExitList : HelpTextElement
    {
        public override void Write(StringBuilder builder)
        { }

        public override void Write(HelpWriter writer)
            => writer.ExitList();
    }
}