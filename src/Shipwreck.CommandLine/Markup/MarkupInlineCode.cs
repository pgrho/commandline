using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    [DebuggerDisplay("`{" + nameof(MarkupInlineCode.Text) + "}`")]
    public sealed class MarkupInlineCode : MarkupTextElement
    {
        /// <summary>
        /// <see cref="MarkupInlineCode" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MarkupInlineCode()
        {
        }

        public MarkupInlineCode(string text)
            : base(text)
        {
        }

        public new MarkupInlineCode Clone()
            => (MarkupInlineCode)base.Clone();

        /// <inheritdoc />
        protected override MarkupObject CreateInstanceCore()
            => new MarkupInlineCode();
    }
}