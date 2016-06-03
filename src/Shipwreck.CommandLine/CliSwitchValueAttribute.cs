using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CliSwitchValueAttribute : Attribute
    {
        public CliSwitchValueAttribute(Type type, string switchValue)
        {
            SwitchValue = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(switchValue);
        }

        public CliSwitchValueAttribute(bool switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(byte switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(short switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(int switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(long switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(float switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(double switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(char switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(string switchValue)
        {
            SwitchValue = switchValue;
        }

        public CliSwitchValueAttribute(object switchValue)
        {
            SwitchValue = switchValue;
        }

        public virtual object SwitchValue { get; }
    }
}