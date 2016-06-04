using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Shipwreck.CommandLine.ObjectModels
{
    [TestClass]
    public class CommandExecuteTest : LoaderTest<TestCliCommand, TestCliCommand>
    {
        protected override TestCliCommand CreateInstance() => new TestCliCommand();

        protected override void ExecuteCore(TestCliCommand root, LoaderSettings settings, params string[] args)
        {
            var r = root.Execute(settings.GetHashCode(), args, settings: settings);
            Assert.AreEqual(root.GetHashCode() + settings.GetHashCode(), r);
        }

        protected override TestCliCommand GetTarget(TestCliCommand root) => root;
    }
}