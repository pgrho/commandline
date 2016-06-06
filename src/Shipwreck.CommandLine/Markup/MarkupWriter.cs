using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class MarkupWriter : IDisposable
    {
        public void Write(MarkupDocument document)
        {
            foreach (var b in document.Blocks)
            {
                var l = b as MarkupList;
                if (l != null)
                {
                    WriteList(l);
                    continue;
                }

                WriteParagraph((MarkupParagraph)b);
            }

            Flush();
        }

        protected abstract void WriteList(MarkupList list);

        protected abstract void WriteParagraph(MarkupParagraph paragraph);

        protected virtual void WriteInlines(IEnumerable<MarkupInline> inlines)
        {
            foreach (var i in inlines)
            {
                var lineBreak = i as MarkupLineBreak;
                if (lineBreak != null)
                {
                    WriteLineBreak(lineBreak);
                    continue;
                }
                var code = i as MarkupInlineCode;
                if (code != null)
                {
                    WriteInlineCode(code);
                    continue;
                }
                WriteRun((MarkupRun)i);
            }
        }

        protected abstract void WriteRun(MarkupRun run);

        protected abstract void WriteInlineCode(MarkupInlineCode code);

        protected abstract void WriteLineBreak(MarkupLineBreak lineBreak);

        public virtual void Flush()
        {
        }

        /// <summary>
        /// インスタンスが破棄されているかどうかを示す値を取得します。
        /// </summary>
        protected bool IsDisposed { get; private set; }

        #region IDisposable メソッド

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable メソッド

        #region デストラクタ

        /// <summary>
        /// オブジェクトがガベジ コレクションにより収集される前に、そのオブジェクトがリソースを解放し、その他のクリーンアップ操作を実行できるようにします。
        /// </summary>
        ~MarkupWriter()
        {
            Dispose(false);
        }

        #endregion デストラクタ

        #region 仮想メソッド

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        /// <param name="disposing">メソッドが<see cref="MarkupWriter.Dispose()" />から呼び出された場合は<c>true</c>。その他の場合は<c>false</c>。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
        }

        #endregion 仮想メソッド
    }
}