using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    internal class MarkupAssert : MarkupVisitor
    {
        private readonly Stack<MarkupObject> _Expectations;
        private readonly Stack<string> _Path;

        private MarkupAssert(MarkupObject expected, int? index = null)
        {
            _Expectations = new Stack<MarkupObject>();
            _Expectations.Push(expected);

            _Path = new Stack<string>();
            if (index == null)
            {
                _Path.Push(expected.GetType().Name);
            }
            else
            {
                _Path.Push(expected.GetType().Name + $"[{index}]");
            }
        }

        public override void Visit(MarkupDocument document)
        {
            var ep = _Expectations.Peek();
            AssertInstanceType(ep, document);

            VisitChildren(ep, document);
        }

        protected override void VisitList(MarkupList list)
        {
            var ep = _Expectations.Peek();
            AssertInstanceType(ep, list);

            VisitChildren(ep, list);
        }

        protected override void VisitListItem(MarkupListItem listItem)
        {
            var ep = _Expectations.Peek();
            AssertInstanceType(ep, listItem);

            VisitChildren(ep, listItem);
        }

        protected override void VisitParagraph(MarkupParagraph paragraph)
        {
            var ep = _Expectations.Peek();
            AssertInstanceType(ep, paragraph);

            VisitChildren(ep, paragraph);
        }

        protected override void VisitRun(MarkupRun run)
        {
            var ep = _Expectations.Peek();
            AssertInstanceType(ep, run);
            Assert.AreEqual(((MarkupRun)ep).Text, run.Text);

            VisitChildren(ep, run);
        }

        protected override void VisitInlineCode(MarkupInlineCode code)
        {
            var ep = _Expectations.Peek();
            AssertInstanceType(ep, code);
            Assert.AreEqual(((MarkupInlineCode)ep).Text, code.Text);

            VisitChildren(ep, code);
        }

        protected override void VisitLineBreak(MarkupLineBreak lineBreak)
        {
            var ep = _Expectations.Peek();
            AssertInstanceType(ep, lineBreak);

            VisitChildren(ep, lineBreak);
        }

        private void AssertInstanceType(MarkupObject expected, MarkupObject actual)
        {
            if (!expected.GetType().IsAssignableFrom(actual.GetType()))
            {
                Assert.Fail($"{string.Join("/", _Path.Reverse())}で{expected.GetType()}が期待されていましたが、実際には{actual.GetType()}が渡されました。");
            }
        }

        private void VisitChildren(MarkupObject expected, MarkupObject actual)
        {
            var ec = expected.GetChildren();
            var ac = actual.GetChildren();
            Assert.AreEqual(ec.Count, ac.Count);

            for (var i = 0; i < ec.Count; i++)
            {
                var ep = ec[i];
                _Expectations.Push(ep);
                _Path.Push($"{ep.GetType().Name}[{i}]");

                Visit(ac[i]);

                _Expectations.Pop();
                _Path.Pop();
            }
        }


        public static void AreEqual(MarkupObject expected, MarkupObject actual)
        {
            var instance = new MarkupAssert(expected);
            instance.Visit(actual);
        }

        public static void AreEqual(IEnumerable<MarkupObject> expected, IEnumerable<MarkupObject> actual)
        {
            var el = expected.ToList();
            var al = actual.ToList();

            Assert.AreEqual(el.Count, al.Count);

            for (int i = 0; i < el.Count; i++)
            {
                var instance = new MarkupAssert(el[i]);
                instance.Visit(al[i]);
            }
        }
    }
}
