using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class HelpListBullet : HelpTextElement
    {
        public override void Write(StringBuilder builder)
            => builder.Append(" * ");

        public override void Write(HelpWriter writer)
            => writer.WriteListBullet();
    }
}