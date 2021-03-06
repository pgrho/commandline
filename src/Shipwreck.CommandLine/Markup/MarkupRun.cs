using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    [DebuggerDisplay("{" + nameof(MarkupRun.Text) + "}")]
    public sealed class MarkupRun : MarkupTextElement
    {
        /// <summary>
        /// <see cref="MarkupRun" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MarkupRun()
        {
        }

        public MarkupRun(string text)
            : base(text)
        {
        }

        public new MarkupRun Clone()
            => (MarkupRun)base.Clone();

        /// <inheritdoc />
        protected override MarkupObject CreateInstanceCore()
            => new MarkupRun();
    }
}