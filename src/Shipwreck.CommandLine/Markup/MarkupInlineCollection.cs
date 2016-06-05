using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class MarkupInlineCollection : MarkupObjectCollection<MarkupInline>
    {
        /// <summary>
        /// <see cref="MarkupInlineCollection" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MarkupInlineCollection()
            : base(null)
        {
        }

        internal MarkupInlineCollection(MarkupObject parent)
            : base(parent)
        {
        }
    }
}