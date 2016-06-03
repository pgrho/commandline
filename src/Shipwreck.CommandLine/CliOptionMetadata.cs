using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public sealed class CliOptionMetadata
    {
        private static readonly ReadOnlyCollection<string> EmptyStrings = Array.AsReadOnly(new string[0]);
        private static readonly Regex NeverMatch = new Regex("$.");

        private CliOptionMetadata(PropertyInfo property)
        {
            Property = property;
            Converter = TypeDescriptor.GetProperties(Property.DeclaringType)[Property.Name].Converter;
            IsIgnored = true;
            Order = -1;
            Names = EmptyStrings;
            NamesPattern = NeverMatch;
        }

        private CliOptionMetadata(PropertyInfo property, int order, ReadOnlyCollection<string> names, bool hasSwitchValue, object switchValue, bool isDefault, int precedence)
        {
            Property = property;
            Converter = TypeDescriptor.GetProperties(Property.DeclaringType)[Property.Name].Converter;
            IsIgnored = false;
            Order = order;
            Names = names;
            NamesPattern = names == EmptyStrings ? NeverMatch
                                    : new Regex("(" + string.Join("|", names.Select(Regex.Escape)) + ")", RegexOptions.IgnoreCase);
            HasSwitchValue = hasSwitchValue;
            SwitchValue = switchValue;
            IsDefault = isDefault;
            Precedence = precedence;
        }

        public PropertyInfo Property { get; }

        public TypeConverter Converter { get; }

        public bool IsIgnored { get; }

        public int Order { get; }

        public ReadOnlyCollection<string> Names { get; }

        public Regex NamesPattern { get; }

        public bool HasSwitchValue { get; }

        public object SwitchValue { get; }

        public bool IsDefault { get; }

        public int Precedence { get; }

        public object GetValue(object target) => Property.GetValue(target);

        public void SetValue(object target, string value)
        {
            if (value == null)
            {
                if (!HasSwitchValue)
                {
                    // TODO:
                }
                else
                {
                    Property.SetValue(target, SwitchValue);
                }
            }
            else if (Property.PropertyType == typeof(string))
            {
                Property.SetValue(target, value);
            }
            else
            {
                Property.SetValue(target, Converter.ConvertFromInvariantString(value));
            }
        }

        internal static CliOptionMetadata FromPropertyInfo(PropertyInfo property)
        {
            var ignored = property.GetCustomAttribute<CliIgnoreAttribute>()?.IsIgnored ?? (property.GetSetMethod(false)?.IsPublic != true);

            if (ignored)
            {
                return new CliOptionMetadata(property);
            }

            var order = property.GetCustomAttribute<CliOrderAttribute>()?.Order ?? -1;

            ReadOnlyCollection<string> ns;
            var names = property.GetCustomAttributes<CliOptionAttribute>().Select(_ => _.Name).OrderBy(_ => _).Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();

            if (names.Any(string.IsNullOrEmpty))
            {
                if (names.Length != 1)
                {
                    throw new Exception();
                }
                ns = EmptyStrings;
            }
            else if (names.Any())
            {
                ns = Array.AsReadOnly(names);
            }
            else
            {
                ns = Array.AsReadOnly(new[] { property.Name });
            }

            var switchAttr = property.GetCustomAttribute<CliSwitchValueAttribute>();

            var defAttr = property.GetCustomAttribute<CliDefaultOptionAttribute>();

            return new CliOptionMetadata(property, order, ns, switchAttr != null, switchAttr?.SwitchValue, defAttr?.IsDefault ?? false, defAttr?.Precedence ?? 0);
        }
    }
}