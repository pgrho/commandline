using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public sealed class ParameterMetadata : CommandOptionMetadata
    {
        internal ParameterMetadata(ParameterInfo parameter)
            : base(parameter.Name, parameter, parameter.ParameterType)
        {
        }

        public ParameterInfo Parameter => (ParameterInfo)Member;
    }
}