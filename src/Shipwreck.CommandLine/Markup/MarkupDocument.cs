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
    }
}