using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class MarkupTextElement : MarkupInline
    {
        /// <summary>
        /// <see cref="Text" />のバッキングストアです。
        /// </summary>
        private string _Text;

        /// <summary>
        /// <see cref="MarkupTextElement" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public MarkupTextElement()
        {
            _Text = string.Empty;
        }

        public MarkupTextElement(string text)
        {
            _Text = text ?? string.Empty;
        }

        /// <summary>
        /// 要素に含まれる文字列を取得または設定します。
        /// </summary>
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                ThrowIfFreezed();
                var v = value ?? string.Empty;
                _Text = v;
            }
        }

        protected override void OnFreeze()
        {
        }

        public static string Escape(string text)
        {
            using (var sw = new StringWriter())
            {
                Escape(sw, text);
                sw.Flush();
                return sw.ToString();
            }
        }

        public static void Escape(TextWriter writer, string text)
        {
            if (text == null)
            {
                return;
            }

            foreach (var c in text)
            {
                if (IsMetaChar(c))
                {
                    writer.Write('\\');
                }
                writer.Write(c);
            }
        }
        /// <inheritdoc />
        public override void CopyTo(MarkupObject other)
        {
            base.CopyTo(other);

            ((MarkupTextElement)other).Text = _Text;
        }
    }
}