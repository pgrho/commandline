using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    internal static class MH
    {
        public static MarkupParagraph ToParagraph(this string text)
        {
            if (text == null)
            {
                return null;
            }

            return MarkupParagraph.FromText(text, freeze: true);
        }

        public static MarkupParagraph Parse(string valueOrKey, Type resourceType)
        {
            var s = RH.GetString(valueOrKey, resourceType);
            if (s == null)
            {
                return null;
            }
            var m = MarkupParagraph.Parse(s);
            m.Freeze();
            return m;
        }

        public static MarkupParagraph Parse(this IMarkupAttribute attribute)
        {
            if (attribute?.MarkupOrResourceKey == null)
            {
                return null;
            }

            var m = MarkupParagraph.Parse(RH.GetString(attribute.MarkupOrResourceKey, attribute.ResourceType));
            m.Freeze();
            return m;
        }
    }
}