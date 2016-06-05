using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class MarkupDocument : MarkupObject
    {
        private MarkupBlockCollection _Blocks;

        public MarkupBlockCollection Blocks
        {
            get
            {
                return _Blocks ?? (_Blocks = new MarkupBlockCollection(this));
            }
            set
            {
                if (value == _Blocks)
                {
                    return;
                }

                _Blocks?.Clear();
                if (value?.Count > 0)
                {
                    var list = Blocks;
                    foreach (var e in value)
                    {
                        list.Add(e);
                    }
                }
            }
        }

        protected override void OnFreeze()
        {
            Blocks.Freeze();
        }

        public static MarkupDocument FromText(string text)
        {
            var md = new MarkupDocument();
            var p = new MarkupParagraph();
            p.Inlines.Add(new MarkupRun(text));
            md.Blocks.Add(p);

            return md;
        }

        public static MarkupDocument Parse(string markup)
        {
            var md = new MarkupDocument();
            MarkupBlock bl = null;
            using (var sr = new StringReader(markup))
            {
                for (var l = sr.ReadLine(); ; l = sr.ReadLine())
                {
                    if (string.IsNullOrWhiteSpace(l))
                    {
                        bl = null;
                        continue;
                    }

                    if (bl == null)
                    {
                        bl = (MarkupBlock)MarkupList.TryParseFirstLine(l)
                                ?? MarkupParagraph.Parse(l);
                    }
                    else
                    {
                        bl.AppendMarkupLine(l);
                    }
                }
            }
        }

        public override string ToString()
        {
            using (var swmw = new StringWriterMarkupWriter())
            {
                swmw.Write(this);
                swmw.Flush();
                return swmw.ToString();
            }
        }
        public new MarkupDocument Clone()
            => (MarkupDocument)base.Clone();

        /// <inheritdoc />
        protected override MarkupObject CreateInstanceCore()
            => new MarkupDocument();

        /// <inheritdoc />
        public override void CopyTo(MarkupObject other)
        {
            base.CopyTo(other);

            var obs = ((MarkupDocument)other).Blocks;
            obs.Clear();

            if (_Blocks != null)
            {
                foreach (var b in _Blocks)
                {
                    obs.Add(b.Clone());
                }
            }
        }
    }
}