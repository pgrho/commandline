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
    public sealed class PropertyMetadata : CommandOptionMetadata
    {
        internal PropertyMetadata(PropertyInfo property)
            : base(property.Name, property, property.PropertyType)
        {
            Property = property;
        }

        public PropertyInfo Property { get; }

        public object GetValue(object target) => Property.GetValue(target);

        public void SetValue(object target, string value)
            => Property.SetValue(target, ConvertFrom(value));
    }
}