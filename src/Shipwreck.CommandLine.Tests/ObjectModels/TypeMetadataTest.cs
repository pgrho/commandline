using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Shipwreck.CommandLine.ObjectModels
{
    [TestClass]
    public class TypeMetadataTest
    {
        #region Combined

        public class TestType1 : Command<object, object>
        {
            public int A { get; set; }

            [CommandLineAliasAttribute("argb")]
            public int B { get; set; }

            [CliSwitchValue(true)]
            public bool C { get; set; }

            [CliDefaultOption(1)]
            public int Def1 { get; set; }

            [CliDefaultOption]
            public string Def2 { get; set; }

            public override object Execute(object parameter)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void SetPropertyTest_01_PropertyName()
        {
            var instance = new TestType1();
            instance.Load(new[] { "-A=1" });
            Assert.AreEqual(1, instance.A);
        }

        [TestMethod]
        public void SetPropertyTest_02_IgnoreCase()
        {
            var instance = new TestType1();
            instance.Load(new[] { "-a=1" });
            Assert.AreEqual(1, instance.A);
        }

        [TestMethod]
        public void SetPropertyTest_03_Alias()
        {
            var instance = new TestType1();
            instance.Load(new[] { "-Argb=1" });
            Assert.AreEqual(1, instance.B);
        }

        [TestMethod]
        public void SetPropertyTest_04_SwitchValue()
        {
            var instance = new TestType1();
            instance.Load(new[] { "-C" });
            Assert.IsTrue(instance.C);
        }

        [TestMethod]
        public void SetPropertyTest_05_DefaultValue()
        {
            var instance = new TestType1();
            instance.Load(new[] { "default" });
            Assert.AreEqual(0, instance.Def1);
            Assert.AreEqual("default", instance.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_06_DefaultValue()
        {
            var instance = new TestType1();
            instance.Load(new[] { "1", "default" });
            Assert.AreEqual(1, instance.Def1);
            Assert.AreEqual("default", instance.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_07_DefaultValue()
        {
            var instance = new TestType1();
            instance.Load(new[] { "1", "default", "default2" });
            Assert.AreEqual(1, instance.Def1);
            Assert.AreEqual("default default2", instance.Def2);
        }

        #endregion

        #region Separated

        [ArgumentStyle(ArgumentStyle.Separated)]
        public class TestType2 : TestType1
        {
        }

        [TestMethod]
        public void SetPropertyTest_8_Separated_PropertyName()
        {
            var instance = new TestType2();
            instance.Load(new[] { "-A", "1" });
            Assert.AreEqual(1, instance.A);
        }

        [TestMethod]
        public void SetPropertyTest_9_Separated_IgnoreCase()
        {
            var instance = new TestType2();
            instance.Load(new[] { "-a", "1" });
            Assert.AreEqual(1, instance.A);
        }

        [TestMethod]
        public void SetPropertyTest_10_Separated_Alias()
        {
            var instance = new TestType2();
            instance.Load(new[] { "-Argb", "1" });
            Assert.AreEqual(1, instance.B);
        }

        [TestMethod]
        public void SetPropertyTest_11_Separated_SwitchValue()
        {
            var instance = new TestType2();
            instance.Load(new[] { "-C" });
            Assert.IsTrue(instance.C);
        }

        [TestMethod]
        public void SetPropertyTest_12_Separated_DefaultValue()
        {
            var instance = new TestType2();
            instance.Load(new[] { "default" });
            Assert.AreEqual(0, instance.Def1);
            Assert.AreEqual("default", instance.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_13_Separated_DefaultValue()
        {
            var instance = new TestType2();
            instance.Load(new[] { "1", "default" });
            Assert.AreEqual(1, instance.Def1);
            Assert.AreEqual("default", instance.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_14_Separated_DefaultValue()
        {
            var instance = new TestType2();
            instance.Load(new[] { "1", "default", "default2" });
            Assert.AreEqual(1, instance.Def1);
            Assert.AreEqual("default default2", instance.Def2);
        }

        #endregion
    }
}