using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class SwitchValueAttribute : Attribute
    {
        public SwitchValueAttribute(Type type, string switchValue)
        {
            SwitchValue = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(switchValue);
        }

        public SwitchValueAttribute(bool switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(byte switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(short switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(int switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(long switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(float switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(double switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(char switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(string switchValue)
        {
            SwitchValue = switchValue;
        }

        public SwitchValueAttribute(object switchValue)
        {
            SwitchValue = switchValue;
        }

        public virtual object SwitchValue { get; }
    }
}