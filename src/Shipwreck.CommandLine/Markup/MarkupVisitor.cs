using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class MarkupVisitor : IDisposable
    {
        public void Visit(MarkupObject obj)
        {
            var i = obj as MarkupInline;
            if (i != null)
            {
                VisitInline(i);
                return;
            }
            var b = obj as MarkupBlock;
            if (b != null)
            {
                VisitBlock(b);
                return;
            }
            Visit((MarkupDocument)obj);
        }

        public virtual void Visit(MarkupDocument document)
        {
            foreach (var b in document.Blocks)
            {
                VisitBlock(b);
            }
        }

        protected virtual void VisitBlock(MarkupBlock block)
        {
            var l = block as MarkupList;
            if (l != null)
            {
                VisitList(l);
                return;
            }

            VisitParagraph((MarkupParagraph)block);
        }

        protected virtual void VisitList(MarkupList list)
        {
            foreach (var li in list.Items)
            {
                VisitListItem(li);
            }
        }

        protected virtual void VisitListItem(MarkupListItem listItem)
        {
            VisitInlines(listItem.Inlines);
        }

        protected virtual void VisitParagraph(MarkupParagraph paragraph)
        {
            VisitInlines(paragraph.Inlines);
        }

        protected virtual void VisitInlines(IEnumerable<MarkupInline> inlines)
        {
            foreach (var i in inlines)
            {
                var lineBreak = i as MarkupLineBreak;
                if (lineBreak != null)
                {
                    VisitLineBreak(lineBreak);
                    continue;
                }
                var code = i as MarkupInlineCode;
                if (code != null)
                {
                    VisitInlineCode(code);
                    continue;
                }
                VisitRun((MarkupRun)i);
            }
        }

        protected virtual void VisitInline(MarkupInline inline)
        {
            var lineBreak = inline as MarkupLineBreak;
            if (lineBreak != null)
            {
                VisitLineBreak(lineBreak);
                return;
            }
            var code = inline as MarkupInlineCode;
            if (code != null)
            {
                VisitInlineCode(code);
                return;
            }
            VisitRun((MarkupRun)inline);
        }

        protected virtual void VisitRun(MarkupRun run) { }

        protected virtual void VisitInlineCode(MarkupInlineCode code) { }

        protected virtual void VisitLineBreak(MarkupLineBreak lineBreak) { }

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
        ~MarkupVisitor()
        {
            Dispose(false);
        }

        #endregion デストラクタ

        #region 仮想メソッド

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        /// <param name="disposing">メソッドが<see cref="MarkupVisitor.Dispose()" />から呼び出された場合は<c>true</c>。その他の場合は<c>false</c>。</param>
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