using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipwreck.CommandLine.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    [TestClass]
    public class PropertyMetadataTest
    {
        public class DT : TestCliCommand
        {
            [DescriptionMarkup(nameof(Expectations.MarkupKey), typeof(Expectations))]
            public int DescriptionProperty1 { get; set; }

            [DescriptionMarkup(nameof(Expectations.MarkupKey))]
            public int DescriptionProperty2 { get; set; }

            [Option(Description = nameof(Expectations.MarkupKey), DescriptionResourceType = typeof(Expectations))]
            public int DescriptionProperty3 { get; set; }

            [Option(Description = nameof(Expectations.MarkupKey))]
            public int DescriptionProperty4 { get; set; }

            [System.ComponentModel.Description(nameof(Expectations.MarkupKey))]
            public int DescriptionProperty5 { get; set; }

            public int DescriptionProperty6 { get; set; }
        }

        [TestMethod]
        public void DesciptionTest_1()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT)).Properties[nameof(DT.DescriptionProperty1)].Description);
        }

        [TestMethod]
        public void DesciptionTest_2()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT)).Properties[nameof(DT.DescriptionProperty2)].Description);
        }

        [TestMethod]
        public void DesciptionTest_3()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT)).Properties[nameof(DT.DescriptionProperty3)].Description);
        }

        [TestMethod]
        public void DesciptionTest_4()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT)).Properties[nameof(DT.DescriptionProperty4)].Description);
        }

        [TestMethod]
        public void DesciptionTest_5()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT)).Properties[nameof(DT.DescriptionProperty5)].Description);
        }

        [TestMethod]
        public void DesciptionTest_6()
        {
            MarkupAssert.AreEqual(
                null,
                TypeMetadata.FromType(typeof(DT)).Properties[nameof(DT.DescriptionProperty6)].Description);
        }
    }
}