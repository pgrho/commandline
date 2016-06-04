using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.CommandLine.ObjectModels
{
    public class TestCliCommand : LoaderTestObject, ICliCommand<object, object>
    {
        public object Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}