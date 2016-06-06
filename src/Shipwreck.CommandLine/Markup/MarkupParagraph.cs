using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public class MarkupParagraph : MarkupBlock
    {
        private MarkupInlineCollection _Inlines;

        public MarkupInlineCollection Inlines
        {
            get
            {
                return _Inlines ?? (_Inlines = new MarkupInlineCollection(this));
            }
            set
            {
                if (value == _Inlines)
                {
                    return;
                }

                _Inlines?.Clear();
                if (value?.Count > 0)
                {
                    var list = Inlines;
                    foreach (var e in value)
                    {
                        list.Add(e);
                    }
                }
            }
        }

        protected override void OnFreeze()
        {
            Inlines.Freeze();
        }

        public override void AppendMarkupLine(string markupLine)
        {
            var list = Inlines;
            if (list.Count > 0)
            {
                list.Add(new MarkupLineBreak());
            }

            foreach (var i in MarkupInline.ParseInlines(markupLine))
            {
                list.Add(i);
            }
        }

        public static MarkupParagraph Parse(string markupLine)
        {
            var r = new MarkupParagraph();
            r.AppendMarkupLine(markupLine);
            return r;
        }
        public new MarkupParagraph Clone()
            => (MarkupParagraph)base.Clone();

        /// <inheritdoc />
        protected override MarkupObject CreateInstanceCore()
            => new MarkupParagraph();

        /// <inheritdoc />
        public override void CopyTo(MarkupObject other)
        {
            base.CopyTo(other);

            var obs = ((MarkupParagraph)other).Inlines;
            obs.Clear();

            if (_Inlines != null)
            {
                foreach (var b in _Inlines)
                {
                    obs.Add(b.Clone());
                }
            }
        }

        public override IReadOnlyList<MarkupObject> GetChildren()
            => Inlines;
    }
}