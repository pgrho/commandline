using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public class TextWriterMarkupWriter : MarkupWriter
    {
        private readonly bool _LeaveOpen;

        public TextWriterMarkupWriter(TextWriter writer) : this(writer, false)
        { }

        public TextWriterMarkupWriter(TextWriter writer, bool leaveOpen)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_LeaveOpen)
            {
                Writer.InnerWriter.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void WriteList(MarkupList list)
        {
            foreach (var li in list.Items)
            {
                Writer.Write(ListBullet);
                var ci = Writer.Indent;
                Writer.Indent += ListIndent;

                WriteInlines(li.Inlines);

                Writer.Indent = ci;
                Writer.WriteLine();
            }
            Writer.WriteLine();
            Writer.WriteLine();
        }

        protected override void WriteParagraph(MarkupParagraph paragraph)
        {
            WriteInlines(paragraph.Inlines);
            Writer.WriteLine();
            Writer.WriteLine();
        }

        public override void Flush()
        {
            Writer.Flush();
        }

        protected override void WriteRun(MarkupRun run)
        {
            Writer.Write(run.Text);
        }

        protected override void WriteInlineCode(MarkupInlineCode code)
        {
            Writer.Write(InlineCodeLeftBracket);
            Writer.Write(code.Text);
            Writer.Write(InlineCodeRightBracket);
        }

        protected override void WriteLineBreak(MarkupLineBreak lineBreak)
        {
            Writer.WriteLine();
        }
    }
}