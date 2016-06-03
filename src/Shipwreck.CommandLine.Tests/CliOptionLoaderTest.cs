using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Shipwreck.CommandLine
{
    [TestClass]
    public class CliOptionLoaderTest
    {
        public class TestType1
        {
            public int A { get; set; }

            [CliOption("argb")]
            public int B { get; set; }

            [CliSwitchValue(true)]
            public bool C { get; set; }
        }

        [TestMethod]
        public void SetPropertyTest_1()
        {
            var instance = new TestType1();
            CliOptionLoader.Load(instance, new[] { "-A=1" });
            Assert.AreEqual(1, instance.A);
        }

        [TestMethod]
        public void SetPropertyTest_2_IgnoreCase()
        {
            var instance = new TestType1();
            CliOptionLoader.Load(instance, new[] { "-a=1" });
            Assert.AreEqual(1, instance.A);
        }

        [TestMethod]
        public void SetPropertyTest_3_Alias()
        {
            var instance = new TestType1();
            CliOptionLoader.Load(instance, new[] { "-Argb=1" });
            Assert.AreEqual(1, instance.B);
        }

        [TestMethod]
        public void SetPropertyTest_4_SwitchValue()
        {
            var instance = new TestType1();
            CliOptionLoader.Load(instance, new[] { "-C" });
            Assert.IsTrue(instance.C);
        }
    }
}