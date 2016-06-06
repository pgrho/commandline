using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.Markup
{
    [TestClass]
    public class MarkupDocumentTest
    {
        [TestMethod]
        public void ParseTest()
        {
            var markup = @"a
b

* c
* d
  e

f";

            var actual = MarkupDocument.Parse(markup);

            MarkupAssert.AreEqual(new MarkupDocument()
            {
                Blocks = new MarkupBlockCollection()
                {
                    new MarkupParagraph()
                    {
                        Inlines = new MarkupInlineCollection ()
                        {
                            new MarkupRun("a"),
                            new MarkupLineBreak(),
                            new MarkupRun("b")
                        }
                    },
                    new MarkupList()
                    {
                        Items = new MarkupListItemCollection ()
                        {
                            new MarkupListItem()
                            {
                                Inlines = new MarkupInlineCollection () { new MarkupRun("c") }
                            },
                            new MarkupListItem()
                            {
                                Inlines = new MarkupInlineCollection ()
                                {
                                    new MarkupRun("d"),
                                    new MarkupLineBreak(),
                                    new MarkupRun("e")
                                }
                            }
                        }
                    },
                    new MarkupParagraph()
                    {
                        Inlines = new MarkupInlineCollection ()
                        {
                            new MarkupRun("f")
                        }
                    }
                }
            }, actual);

            //Assert.AreEqual(3, md.Blocks.Count);

            //Assert.IsInstanceOfType(md.Blocks[0], typeof(MarkupParagraph));
            //Assert.IsInstanceOfType(md.Blocks[1], typeof(MarkupList));
            //Assert.IsInstanceOfType(md.Blocks[2], typeof(MarkupParagraph));
        }
    }
}
