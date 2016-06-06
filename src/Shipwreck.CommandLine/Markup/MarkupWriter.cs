using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public class MarkupWriter : MarkupVisitor
    {
        private readonly bool _LeaveOpen;

        public MarkupWriter(TextWriter writer) : this(writer, false)
        { }

        public MarkupWriter(TextWriter writer, bool leaveOpen)
        {
            Writer = new IndentedTextWriter(writer, " ");
            _LeaveOpen = leaveOpen;

            ListBullet = "* ";
            ListIndent = 2;
            InlineCodeLeftBracket = "`";
            InlineCodeRightBracket = "`";
        }

        protected IndentedTextWriter Writer { get; }

        public string ListBullet { get; set; }

        public int ListIndent { get; set; }

        public string InlineCodeLeftBracket { get; set; }
        public string InlineCodeRightBracket { get; set; }

        public override void Visit(MarkupDocument document)
        {
            base.Visit(document);
            Flush();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_LeaveOpen)
            {
                Writer.InnerWriter.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void VisitList(MarkupList list)
        {
            foreach (var li in list.Items)
            {
                VisitListItem(li);
            }
            Writer.WriteLine();
            Writer.WriteLine();
        }

        protected override void VisitListItem(MarkupListItem listItem)
        {
            Writer.Write(ListBullet);
            var ci = Writer.Indent;
            Writer.Indent += ListIndent;

            VisitInlines(listItem.Inlines);

            Writer.Indent = ci;
            Writer.WriteLine();
        }

        protected override void VisitParagraph(MarkupParagraph paragraph)
        {
            VisitInlines(paragraph.Inlines);
            Writer.WriteLine();
            Writer.WriteLine();
        }

        public void Flush()
        {
            Writer.Flush();
        }

        protected override void VisitRun(MarkupRun run)
        {
            Writer.Write(run.Text);
        }

        protected override void VisitInlineCode(MarkupInlineCode code)
        {
            Writer.Write(InlineCodeLeftBracket);
            Writer.Write(code.Text);
            Writer.Write(InlineCodeRightBracket);
        }

        protected override void VisitLineBreak(MarkupLineBreak lineBreak)
        {
            Writer.WriteLine();
        }
    }
}