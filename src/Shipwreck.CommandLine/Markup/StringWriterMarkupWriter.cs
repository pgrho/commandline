using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class StringWriterMarkupWriter : TextWriterMarkupWriter
    {
        /// <summary>
        /// <see cref="StringWriterMarkupWriter" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public StringWriterMarkupWriter()
            : base(new StringWriter())
        {
        }

        public override string ToString()
            => IsDisposed ? base.ToString() : Writer.InnerWriter.ToString();
    }
}