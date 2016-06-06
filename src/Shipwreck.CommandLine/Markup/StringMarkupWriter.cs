using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    internal sealed class StringMarkupWriter : MarkupWriter
    {
        /// <summary>
        /// <see cref="StringMarkupWriter" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public StringMarkupWriter()
            : base(new StringWriter())
        {
            InlineCodeLeftBracket = InlineCodeRightBracket = string.Empty;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (IsDisposed)
            {
                return base.ToString();
            }
            Writer.Flush();
            var sb = ((StringWriter)Writer.InnerWriter).GetStringBuilder();
            while (sb.Length > 0)
            {
                var i = sb.Length - 1;
                if (!char.IsWhiteSpace(sb[i]))
                {
                    break;
                }
                sb.Length -= 1;
            }
            return sb.ToString();
        }
    }
}