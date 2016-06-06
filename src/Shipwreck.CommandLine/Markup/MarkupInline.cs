using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    public abstract class MarkupInline : MarkupObject
    {

        private enum InlineType
        {
            Run,
            InlineCode
        }

        public new MarkupInline Clone()
            => (MarkupInline)base.Clone();

        public static IEnumerable<MarkupInline> ParseInlines(string markupLine)
        {
            var s = InlineType.Run;
            var escaped = false;
            var sb = new StringBuilder();
            var lbAdded = false;
            foreach (var c in markupLine)
            {
                if (c == '\r' || c == '\n')
                {
                    if (sb.Length > 0)
                    {
                        yield return CreateInline(s, sb);

                        s = InlineType.Run;
                    }
                    if (!lbAdded)
                    {
                        yield return new MarkupLineBreak();
                        lbAdded = true;
                    }
                    continue;
                }
                lbAdded = false;

                if (escaped && IsMetaChar(c))
                {
                    sb.Append(c);
                    escaped = false;
                    continue;
                }
                if (c == '\\')
                {
                    escaped = true;
                    continue;
                }
                escaped = false;

                switch (s)
                {
                    case InlineType.Run:
                        switch (c)
                        {
                            case '`':
                                if (sb.Length > 0)
                                {
                                    yield return CreateInline(s, sb);
                                }
                                s = InlineType.InlineCode;
                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                        break;
                    case InlineType.InlineCode:
                        switch (c)
                        {
                            case '`':
                                yield return CreateInline(s, sb);

                                s = InlineType.Run;
                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                        break;
                }
            }

            if (sb.Length > 0)
            {
                yield return CreateInline(s, sb);
            }
        }

        private static MarkupInline CreateInline(InlineType s, StringBuilder sb)
        {
            var text = sb.ToString();
            sb.Clear();

            switch (s)
            {
                case InlineType.Run:
                    return new MarkupRun(text);

                case InlineType.InlineCode:
                    return new MarkupInlineCode(text);

            }
            throw new NotImplementedException();
        }

        public override IReadOnlyList<MarkupObject> GetChildren()
            => EmptyObjects;
    }
}