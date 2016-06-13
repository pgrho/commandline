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
    public class ParameterMetadataTest
    {
        #region Description

        public class DT : TestCliCommand
        {
            [Command]
            public object M(
                [DescriptionMarkup(nameof(Expectations.MarkupKey), typeof(Expectations))]
                int p1,
                [DescriptionMarkup(nameof(Expectations.MarkupKey))]
                int p2,
                [Option(Description = nameof(Expectations.MarkupKey), DescriptionResourceType = typeof(Expectations))]
                int p3,
                [Option(Description = nameof(Expectations.MarkupKey))]
                int p4,
                [System.ComponentModel.Description(nameof(Expectations.MarkupKey))]
                int p5,
                int p6) => null;
        }

        [TestMethod]
        public void DescriptionTest_1()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Commands[nameof(DT.M)]
                    .GetOptions().FindByName("p1").Description);
        }

        [TestMethod]
        public void DescriptionTest_2()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Commands[nameof(DT.M)]
                    .GetOptions().FindByName("p2").Description);
        }

        [TestMethod]
        public void DescriptionTest_3()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Commands[nameof(DT.M)]
                    .GetOptions().FindByName("p3").Description);
        }

        [TestMethod]
        public void DescriptionTest_4()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Commands[nameof(DT.M)]
                    .GetOptions().FindByName("p4").Description);
        }

        [TestMethod]
        public void DescriptionTest_5()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT))
                    .Commands[nameof(DT.M)]
                    .GetOptions().FindByName("p5").Description);
        }

        [TestMethod]
        public void DescriptionTest_6()
        {
            MarkupAssert.AreEqual(
                null,
                TypeMetadata.FromType(typeof(DT))
                    .Commands[nameof(DT.M)]
                    .GetOptions().FindByName("p6").Description);
        }

        #endregion Description

        #region ValueDescription

        public class VT : TestCliCommand
        {
            [Command]
            public object M(
                [ValueMarkup(nameof(Expectations.MarkupKey), typeof(Expectations))]
                int p1,
                [ValueMarkup(nameof(Expectations.MarkupKey))]
                int p2,
                [Option(ValueDescription = nameof(Expectations.MarkupKey), ValueDescriptionResourceType = typeof(Expectations))]
                int p3,
                [Option(ValueDescription = nameof(Expectations.MarkupKey))]
                int p4,
                int p5) => null;
        }

        [TestMethod]
        public void ValueDescriptionTest_1()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Commands[nameof(VT.M)]
                    .GetOptions().FindByName("p1").ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_2()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Commands[nameof(VT.M)]
                    .GetOptions().FindByName("p2").ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_3()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Commands[nameof(VT.M)]
                    .GetOptions().FindByName("p3").ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_4()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(VT))
                    .Commands[nameof(VT.M)]
                    .GetOptions().FindByName("p4").ValueDescription);
        }

        [TestMethod]
        public void ValueDescriptionTest_5()
        {
            MarkupAssert.AreEqual(
                null,
                TypeMetadata.FromType(typeof(VT))
                    .Commands[nameof(VT.M)]
                    .GetOptions().FindByName("p5").ValueDescription);
        }

        #endregion ValueDescription
    }
}