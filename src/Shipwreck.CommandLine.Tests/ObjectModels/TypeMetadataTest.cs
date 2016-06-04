using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Shipwreck.CommandLine.ObjectModels
{
    [TestClass]
    public class TypeMetadataTest : LoaderTest<TestCliCommand, TestCliCommand>
    {
        protected override TestCliCommand CreateInstance() => new TestCliCommand();

        protected override void ExecuteCore(TestCliCommand root, LoaderSettings settings, params string[] args)
        {
            root.Load(args, settings: settings);
        }

        protected override TestCliCommand GetTarget(TestCliCommand root) => root;

    }
}