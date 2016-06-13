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
        #region Description

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
        public void DescriptionTest_1()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Properties[nameof(DT.DescriptionProperty1)].Description);
        }

        [TestMethod]
        public void DescriptionTest_2()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Properties[nameof(DT.DescriptionProperty2)].Description);
        }

        [TestMethod]
        public void DescriptionTest_3()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Properties[nameof(DT.DescriptionProperty3)].Description);
        }

        [TestMethod]
        public void DescriptionTest_4()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Properties[nameof(DT.DescriptionProperty4)].Description);
        }

        [TestMethod]
        public void DescriptionTest_5()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Properties[nameof(DT.DescriptionProperty5)].Description);
        }

        [TestMethod]
        public void DescriptionTest_6()
        {
            MarkupAssert.AreEqual(
                null,
                TypeMetadata.FromType(typeof(DT))
                    .Properties[nameof(DT.DescriptionProperty6)].Description);
        }

        #endregion Description

        #region ValueDescription

        public class VT : TestCliCommand
        {
            [ValueMarkup(nameof(Expectations.MarkupKey), typeof(Expectations))]
            public int ValueDescriptionProperty1 { get; set; }

            [ValueMarkup(nameof(Expectations.MarkupKey))]
            public int ValueDescriptionProperty2 { get; set; }

            [Option(ValueDescription = nameof(Expectations.MarkupKey), ValueDescriptionResourceType = typeof(Expectations))]
            public int ValueDescriptionProperty3 { get; set; }

            [Option(ValueDescription = nameof(Expectations.MarkupKey))]
            public int ValueDescriptionProperty4 { get; set; }

            public int ValueDescriptionProperty5 { get; set; }
        }

        [TestMethod]
        public void ValueDescriptionTest_1()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Properties[nameof(VT.ValueDescriptionProperty1)].ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_2()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Properties[nameof(VT.ValueDescriptionProperty2)].ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_3()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Properties[nameof(VT.ValueDescriptionProperty3)].ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_4()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Properties[nameof(VT.ValueDescriptionProperty4)].ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_5()
        {
            MarkupAssert.AreEqual(
                null,
                TypeMetadata.FromType(typeof(VT))
                    .Properties[nameof(VT.ValueDescriptionProperty5)].ValueDescription);
        }

        #endregion ValueDescription
    }
}