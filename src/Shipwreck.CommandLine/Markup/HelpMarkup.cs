using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public sealed class HelpMarkup : IReadOnlyList<HelpTextElement>
    {
        private readonly List<HelpTextElement> _Elements = new List<HelpTextElement>();

        private bool _IsFreezed;

        public int Count
            => _Elements.Count;

        public HelpTextElement this[int index]
            => _Elements[index];

        public HelpMarkup Text(string message)
            => Add(new HelpText(message));

        public HelpMarkup Code(string message)
            => Add(new HelpCode(message));

        public HelpMarkup ListBullet() => Add(new HelpListBullet());

        public HelpMarkup EnterList() => Add(new HelpEnterList());

        public HelpMarkup ExitList() => Add(new HelpExitList());

        public HelpMarkup NewLine() => Add(new HelpNewLine());

        public HelpMarkup Add(HelpTextElement element)
        {
            if (_IsFreezed)
            {
                throw new InvalidOperationException();
            }
            _Elements.Add(element);
            return this;
        }

        public HelpMarkup Freeze()
        {
            _IsFreezed = true;
            return this;
        }

        public IEnumerator<HelpTextElement> GetEnumerator()
            => _Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _Elements.GetEnumerator();

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var e in _Elements)
            {
                e.Write(sb);
            }

            return sb.ToString();
        }
    }
}