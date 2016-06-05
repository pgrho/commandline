using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class MarkupCode : MarkupTextElement
    {
        /// <summary>
        /// <see cref="MarkupCode" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MarkupCode()
        {
        }

        public MarkupCode(string text)
            : base(text)
        {
        }
    }
}