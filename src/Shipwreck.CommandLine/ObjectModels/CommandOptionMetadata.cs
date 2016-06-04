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

    public abstract class CommandOptionMetadata : INamedMetadata
    {
        private readonly MemberNameStore _NameStore;

        private TypeMetadata _TypeMetadata;

        internal CommandOptionMetadata(string memberName, ICustomAttributeProvider member, TypeConverter converter)
        {
            Converter = converter;
            var attr = member.GetCustomAttribute<CommandLineOptionAttribute>();

            IsIgnored = member.GetCustomAttribute<CommandLineIgnoreAttribute>()?.IsIgnored ?? attr?.IsIgnored ?? false;
            Order = member.GetCustomAttribute<CliOrderAttribute>()?.Order ?? -1;

            _NameStore = new MemberNameStore(memberName, member, false);

            var switchAttr = member.GetCustomAttribute<CliSwitchValueAttribute>();
            if (switchAttr != null)
            {
                HasSwitchValue = true;
                SwitchValue = switchAttr.SwitchValue;
            }

            var defAttr = member.GetCustomAttribute<CliDefaultOptionAttribute>();

            IsDefault = defAttr?.IsDefault ?? false;
            Precedence = defAttr?.Precedence ?? -1;
        }

        public abstract Type Type { get; }

        public TypeMetadata TypeMetadata
        {
            get
            {
                if (_TypeMetadata == null)
                {
                    _TypeMetadata = TypeMetadata.FromType(Type);
                }
                return _TypeMetadata;
            }
        }

        public TypeConverter Converter { get; }
        public bool IsIgnored { get; }
        public int Order { get; }

        public string Name => _NameStore.Name;

        public ReadOnlyCollection<string> Names => _NameStore.Names;

        public Regex NamesPattern => _NameStore.NamesPattern;

        public bool HasSwitchValue { get; }

        public object SwitchValue { get; }

        public bool IsDefault { get; }

        public int Precedence { get; }

        public object ConvertFrom(string value)
        {
            if (value == null)
            {
                if (!HasSwitchValue)
                {
                    // TODO:
                    throw new Exception();
                }
                else
                {
                    return SwitchValue;
                }
            }
            else if (Type == typeof(string))
            {
                return value;
            }
            else
            {
                return Converter.ConvertFromInvariantString(value);
            }
        }
    }
}