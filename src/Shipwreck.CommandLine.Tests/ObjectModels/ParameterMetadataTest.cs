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
        public void DesciptionTest_1()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT)).Commands[nameof(DT.M)].GetOptions().FindByName("p1").Description);
        }

        [TestMethod]
        public void DesciptionTest_2()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT)).Commands[nameof(DT.M)].GetOptions().FindByName("p2").Description);
        }

        [TestMethod]
        public void DesciptionTest_3()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedParagraph,
                TypeMetadata.FromType(typeof(DT)).Commands[nameof(DT.M)].GetOptions().FindByName("p3").Description);
        }

        [TestMethod]
        public void DesciptionTest_4()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT)).Commands[nameof(DT.M)].GetOptions().FindByName("p4").Description);
        }

        [TestMethod]
        public void DesciptionTest_5()
        {
            MarkupAssert.AreEqual(
                TH.ExpectedRawParagraph,
                TypeMetadata.FromType(typeof(DT)).Commands[nameof(DT.M)].GetOptions().FindByName("p5").Description);
        }

        [TestMethod]
        public void DesciptionTest_6()
        {
            MarkupAssert.AreEqual(
                null,
                TypeMetadata.FromType(typeof(DT)).Commands[nameof(DT.M)].GetOptions().FindByName("p6").Description);
        }
    }
}