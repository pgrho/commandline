using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class ConsoleMarkupWriter : MarkupWriter
    {
        /// <summary>
        /// <see cref="ConsoleMarkupWriter" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ConsoleMarkupWriter()
            : base(Console.Out)
        {
            InlineCodeLeftBracket = InlineCodeRightBracket = string.Empty;
        }

        /// <inheritdoc />
        protected override void VisitInlineCode(MarkupInlineCode code)
        {
            var f = Console.ForegroundColor;
            var b = Console.BackgroundColor;

            Console.ForegroundColor = b;
            Console.BackgroundColor = f;

            base.VisitInlineCode(code);

            Console.ForegroundColor = f;
            Console.BackgroundColor = b;
        }
    }
}