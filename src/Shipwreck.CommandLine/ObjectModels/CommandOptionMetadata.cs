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

        internal CommandOptionMetadata(string memberName, ICustomAttributeProvider member, Type memberType)
        {
            Type = memberType;
            var convAttr = member.GetCustomAttribute<TypeConverterAttribute>();
            if (convAttr == null)
            {
                Converter = TypeDescriptor.GetConverter(memberType);
            }
            else
            {
                Converter = (TypeConverter)Activator.CreateInstance(Type.GetType(convAttr.ConverterTypeName));
            }
            var attr = member.GetCustomAttribute<OptionAttribute>();

            IsIgnored = member.GetCustomAttribute<IgnoreAttribute>()?.IsIgnored ?? attr?.IsIgnored ?? false;

            Order = member.GetCustomAttribute<OptionOrderAttribute>()?.Order
                    ?? attr?.Order
                    ?? -1;

            _NameStore = new MemberNameStore(memberName, member, false);

            var switchAttr = member.GetCustomAttribute<SwitchValueAttribute>();
            if (switchAttr != null)
            {
                HasSwitchValue = true;
                SwitchValue = switchAttr.SwitchValue;
            }

            var anonymousAttr = member.GetCustomAttribute<AllowAnonymousAttribute>();

            AllowAnonymous = anonymousAttr?.AllowAnonymous
                                ?? attr?.AllowAnonymous
                                ?? false;
            AnonymousPrecedence = anonymousAttr?.AnonymousPrecedence
                                ?? attr?.AnonymousPrecedence
                                ?? -1;
        }

        public Type Type { get; }

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

        /// <summary>
        /// プロパティが無名オプションとなりうるかどうかを示す値を取得します。
        /// </summary>
        public bool AllowAnonymous { get; }

        /// <summary>
        /// 無名オプション内のプロパティの優先順位を取得します。
        /// </summary>
        public int AnonymousPrecedence { get; }

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