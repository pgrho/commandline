using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.CommandLine.ObjectModels
{
    public abstract class LoaderTest<TRoot, TTarget>
        where TTarget : LoaderTestObject
    {
        protected abstract TRoot CreateInstance();

        protected abstract void ExecuteCore(TRoot root, LoaderSettings settings, params string[] args);

        protected abstract TTarget GetTarget(TRoot root);

        #region Combined

        private static LoaderSettings CreateCombinedSettings() => new LoaderSettings(ArgumentStyle.Combined, new[] { "-" }, new[] { "=" });

        [TestMethod]
        public void SetPropertyTest_01_PropertyName()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateCombinedSettings(), "-A=1");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.A);
        }

        [TestMethod]
        public void SetPropertyTest_02_IgnoreCase()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateCombinedSettings(), "-a=1");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.A);
        }

        [TestMethod]
        public void SetPropertyTest_03_Alias()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateCombinedSettings(), "-Argb=1");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.B);
        }

        [TestMethod]
        public void SetPropertyTest_04_SwitchValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateCombinedSettings(), "-C");
            var target = GetTarget(instance);
            Assert.IsTrue(target.C);
        }

        [TestMethod]
        public void SetPropertyTest_05_DefaultValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateCombinedSettings(), "default");
            var target = GetTarget(instance);
            Assert.AreEqual(0, target.Def1);
            Assert.AreEqual("default", target.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_06_DefaultValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateCombinedSettings(), "1", "default");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.Def1);
            Assert.AreEqual("default", target.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_07_DefaultValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateCombinedSettings(), "1", "default", "default2");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.Def1);
            Assert.AreEqual("default default2", target.Def2);
        }

        #endregion Combined

        #region Separated

        private static LoaderSettings CreateSeparatedSettings() => new LoaderSettings(ArgumentStyle.Separated, new[] { "-" }, new string[0]);

        [TestMethod]
        public void SetPropertyTest_8_Separated_PropertyName()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateSeparatedSettings(), "-A", "1");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.A);
        }

        [TestMethod]
        public void SetPropertyTest_9_Separated_IgnoreCase()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateSeparatedSettings(), "-a", "1");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.A);
        }

        [TestMethod]
        public void SetPropertyTest_10_Separated_Alias()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateSeparatedSettings(), "-Argb", "1");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.B);
        }

        [TestMethod]
        public void SetPropertyTest_11_Separated_SwitchValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateSeparatedSettings(), "-C"); ;
            var target = GetTarget(instance);
            Assert.IsTrue(target.C);
        }

        [TestMethod]
        public void SetPropertyTest_12_Separated_DefaultValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateSeparatedSettings(), "default");
            var target = GetTarget(instance);
            Assert.AreEqual(0, target.Def1);
            Assert.AreEqual("default", target.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_13_Separated_DefaultValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateSeparatedSettings(), "1", "default");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.Def1);
            Assert.AreEqual("default", target.Def2);
        }

        [TestMethod]
        public void SetPropertyTest_14_Separated_DefaultValue()
        {
            var instance = CreateInstance();
            ExecuteCore(instance, CreateSeparatedSettings(), "1", "default", "default2");
            var target = GetTarget(instance);
            Assert.AreEqual(1, target.Def1);
            Assert.AreEqual("default default2", target.Def2);
        }

        #endregion Separated
    }
}