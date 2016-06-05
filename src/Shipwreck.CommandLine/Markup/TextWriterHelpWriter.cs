using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public class TextWriterHelpWriter : HelpWriter
    {
        private readonly TextWriter _Writer;

        private bool _HasLine;

        public TextWriterHelpWriter(TextWriter writer)
        {
            _Writer = writer;
        }

        public string ListBullet { get; set; } = "  * ";

        public string ListIndent { get; set; } = "    ";

        public string NewLine { get; set; } = Environment.NewLine;
        public string ListNewLine { get; set; } = Environment.NewLine;

        public override void Write(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            if (!_HasLine)
            {
                _Writer.Write(ListIndent);
            }
            _Writer.Write(message);
            _HasLine = true;
        }

        public override void WriteCode(string message) => Write(message);

        public override void WriteListBullet()
        {
            if (_HasLine)
            {
                throw new InvalidOperationException();
            }

            _Writer.Write(ListBullet);
            _HasLine = true;
        }

        public override void WriteLine()
        {
            if (IsInList)
            {
                _Writer.Write(ListNewLine);
            }
            else
            {
                _Writer.Write(NewLine);
            }
            _HasLine = false;
        }

        public override void Flush()
            => _Writer.Flush();
    }
}