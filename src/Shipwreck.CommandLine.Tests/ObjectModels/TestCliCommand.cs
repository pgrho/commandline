using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.CommandLine.ObjectModels
{
    public class TestCliCommand : LoaderTestObject, ICliCommand<int, int>
    {
        object ICliCommand.Execute(object parameter)
            => Execute((int)parameter);

        public int Execute(int parameter)
            => GetHashCode() + parameter;
    }
}