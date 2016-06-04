using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.CommandLine.ObjectModels
{
    public abstract class LoaderTestObject
    {
        public int A { get; set; }

        [CommandLineAliasAttribute("argb")]
        public int B { get; set; }

        [SwitchValue(true)]
        public bool C { get; set; }

        [AllowAnonymous(1)]
        public int Def1 { get; set; }

        [AllowAnonymous]
        public string Def2 { get; set; }
    }
}