using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class MarkupList : MarkupBlock
    {
        internal static readonly Regex ListBulletPattern = new Regex(@"^\s*[-*+]\s+");

        private MarkupListItemCollection _Items;

        public MarkupListItemCollection Items
        {
            get
            {
                return _Items ?? (_Items = new MarkupListItemCollection(this));
            }
            set
            {
                if (value == _Items)
                {
                    return;
                }

                _Items?.Clear();
                if (value?.Count > 0)
                {
                    var list = Items;
                    foreach (var e in value)
                    {
                        list.Add(e);
                    }
                }
            }
        }

        protected override void OnFreeze()
        {
            Items.Freeze();
        }

        internal static MarkupList TryParseFirstLine(string markup)
        {
            var m = ListBulletPattern.Match(markup);
            if (!m.Success)
            {
                return null;
            }

            var r = new MarkupList();
            var li = new MarkupListItem();
            li.AppendMarkupLine(markup.Substring(m.Length));
            r.Items.Add(li);

            return r;
        }

        public override void AppendMarkupLine(string markupLine)
        {
            var m = ListBulletPattern.Match(markupLine);
            if (m.Success)
            {
                var li = new MarkupListItem();
                li.AppendMarkupLine(markupLine.Substring(m.Length));
                Items.Add(li);
            }
            else
            {
                Items.Last().AppendMarkupLine(markupLine);
            }
        }
        public new MarkupList Clone()
            => (MarkupList)base.Clone();

        /// <inheritdoc />
        protected override MarkupObject CreateInstanceCore()
            => new MarkupList();

        /// <inheritdoc />
        public override void CopyTo(MarkupObject other)
        {
            base.CopyTo(other);

            var obs = ((MarkupList)other).Items;
            obs.Clear();

            if (_Items != null)
            {
                foreach (var b in _Items)
                {
                    obs.Add(b.Clone());
                }
            }
        }
    }
}