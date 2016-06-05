using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class ConsoleMarkupWriter : TextWriterMarkupWriter
    {
        /// <summary>
        /// <see cref="ConsoleMarkupWriter" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ConsoleMarkupWriter()
            : base(Console.Out)
        {
        }
    }
}