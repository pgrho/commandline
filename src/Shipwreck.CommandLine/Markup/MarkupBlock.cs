using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class MarkupBlock : MarkupObject
    {
        public abstract void AppendMarkupLine(string markupLine);
    }
}