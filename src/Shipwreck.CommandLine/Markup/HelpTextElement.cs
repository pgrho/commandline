using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class HelpTextElement
    {
        public abstract void Write(StringBuilder builder);

        public abstract void Write(HelpWriter writer);
    }
}