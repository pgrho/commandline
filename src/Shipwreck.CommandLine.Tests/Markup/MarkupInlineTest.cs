using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    [TestClass]
    public class MarkupInlineTest
    {
        [TestMethod]
        public void ParseInlinesTest()
        {
            var markup = @"a\\b\`c`d\\e\`f`\g
h`i`";
            var inlines = MarkupInline.ParseInlines(markup).ToArray();

            Assert.AreEqual(6, inlines.Length);

            Assert.IsInstanceOfType(inlines[0], typeof(MarkupRun));
            Assert.AreEqual(@"a\b`c", ((MarkupRun)inlines[0]).Text);

            Assert.IsInstanceOfType(inlines[1], typeof(MarkupInlineCode));
            Assert.AreEqual(@"d\e`f", ((MarkupInlineCode)inlines[1]).Text);

            Assert.IsInstanceOfType(inlines[2], typeof(MarkupRun));
            Assert.AreEqual(@"g", ((MarkupRun)inlines[2]).Text);

            Assert.IsInstanceOfType(inlines[3], typeof(MarkupLineBreak));

            Assert.IsInstanceOfType(inlines[4], typeof(MarkupRun));
            Assert.AreEqual(@"h", ((MarkupRun)inlines[4]).Text);

            Assert.IsInstanceOfType(inlines[5], typeof(MarkupInlineCode));
            Assert.AreEqual(@"i", ((MarkupInlineCode)inlines[5]).Text);
        }
    }
}
