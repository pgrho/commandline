using Shipwreck.CommandLine.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    internal static class TH
    {
        public static readonly MarkupParagraph ExpectedParagraph = new MarkupParagraph()
        {
            Inlines = new MarkupInlineCollection()
            {
                new MarkupRun("This is "),
                new MarkupInlineCode("MarkupParagraph"),
                new MarkupRun(".")
            }
        };
        public static readonly MarkupParagraph ExpectedRawParagraph = new MarkupParagraph()
        {
            Inlines = new MarkupInlineCollection()
            {
                new MarkupRun(nameof(Expectations.MarkupKey))
            }
        };
    }
}
