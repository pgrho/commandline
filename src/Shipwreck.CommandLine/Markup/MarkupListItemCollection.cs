using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class MarkupListItemCollection : MarkupObjectCollection<MarkupListItem>
    {
        /// <summary>
        /// <see cref="MarkupListItemCollection" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MarkupListItemCollection()
            : base(null)
        {
        }

        internal MarkupListItemCollection(MarkupObject parent)
            : base(parent)
        {
        }
    }
}