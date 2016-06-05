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
            Text,
            Code
        }

        public static IEnumerable<MarkupInline> ParseInlines(string markupLine)
        {
            var s = InlineType.Code;
            var escaped = false;
            var sb = new StringBuilder();

            foreach (var c in markupLine)
            {
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
                    case InlineType.Text:
                        switch (c)
                        {
                            case '[':
                                if (sb.Length > 0)
                                {
                                    yield return new MarkupRun(sb.ToString());
                                    sb.Clear();
                                }
                                s = InlineType.Code;
                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                        break;
                    case InlineType.Code:
                        switch (c)
                        {
                            case ']':
                                yield return new MarkupCode(sb.ToString());
                                sb.Clear();
                                s = InlineType.Text;
                                break;
                            default:
                                sb.Append(c);
                                break;
                        }
                        break;
                }
            }
        }
    }
}