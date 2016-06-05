using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class MarkupBlockCollection : MarkupObjectCollection<MarkupBlock>
    {
        /// <summary>
        /// <see cref="MarkupBlockCollection" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MarkupBlockCollection()
            : base(null)
        {
        }

        internal MarkupBlockCollection(MarkupObject parent)
            : base(parent)
        {
        }
    }
}