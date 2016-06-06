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

        #region 遅延読み込み状態

        /// <summary>
        /// <see cref="_IsLoaded" />のロックです。
        /// </summary>
        private readonly object _LoadingLock;

        /// <summary>
        /// <see cref="LoadCore" />が実行されたかどうかを示す値です。
        /// </summary>
        private bool _IsLoaded;

        #endregion Load

        #region 遅延読み込みプロパティ

        private TypeMetadata _TypeMetadata;
        private TypeConverter _Converter;

        /// <summary>
        /// <see cref="IsIgnored" />のバッキングストアです。
        /// </summary>
        private bool _IsIgnored;

        /// <summary>
        /// <see cref="Order" />のバッキングストアです。
        /// </summary>
        private int _Order;

        private bool _HasSwitchValue;

        private object _SwitchValue;
        private bool _AllowAnonymous;

        private int _AnonymousPrecedence;

        #endregion 遅延読み込みプロパティ

        internal CommandOptionMetadata(string memberName, ICustomAttributeProvider member, Type memberType)
        {
            Type = memberType;
            _NameStore = new MemberNameStore(memberName, member, false);

            _LoadingLock = new object();
        }

        public Type Type { get; }

        public string Name => _NameStore.Name;

        public ReadOnlyCollection<string> Names => _NameStore.Names;

        public Regex NamesPattern => _NameStore.NamesPattern;

        protected ICustomAttributeProvider Member => _NameStore.Member;

        #region 遅延読み込みプロパティ

        public TypeMetadata TypeMetadata
            => EnsureLoaded()._TypeMetadata;

        public TypeConverter Converter
            => EnsureLoaded()._Converter;

        /// <summary>
        /// メンバーが無視されるかどうかを示す値を取得します。
        /// </summary>
        public bool IsIgnored
            => EnsureLoaded()._IsIgnored;

        /// <summary>
        /// メンバーの処理順を取得します。
        /// </summary>
        public int Order
            => EnsureLoaded()._Order;

        public bool HasSwitchValue
            => EnsureLoaded()._HasSwitchValue;

        public object SwitchValue
            => EnsureLoaded()._SwitchValue;

        /// <summary>
        /// プロパティが無名オプションとなりうるかどうかを示す値を取得します。
        /// </summary>
        public bool AllowAnonymous
            => EnsureLoaded()._AllowAnonymous;

        /// <summary>
        /// 無名オプション内のプロパティの優先順位を取得します。
        /// </summary>
        public int AnonymousPrecedence
            => EnsureLoaded()._AnonymousPrecedence;

        #endregion 遅延読み込みプロパティ

        #region Load

        /// <summary>
        /// 必要であればメタデータを読み込みます。
        /// </summary>
        /// <returns>現在のインスタンス。</returns>
        protected CommandOptionMetadata EnsureLoaded()
        {
            lock (_LoadingLock)
            {
                if (_IsLoaded)
                {
                    return this;
                }

                LoadCore();

                _IsLoaded = true;
            }
            return this;
        }

        /// <summary>
        /// 読み込み処理を実行します。
        /// </summary>
        protected virtual void LoadCore()
        {
            _TypeMetadata = TypeMetadata.FromType(Type);

            var convAttr = Member.GetCustomAttribute<TypeConverterAttribute>();
            if (convAttr == null)
            {
                _Converter = TypeDescriptor.GetConverter(Type);
            }
            else
            {
                _Converter = (TypeConverter)Activator.CreateInstance(Type.GetType(convAttr.ConverterTypeName));
            }

            var attr = Member.GetCustomAttribute<OptionAttribute>();

            _IsIgnored = Member.GetCustomAttribute<IgnoreAttribute>()?.IsIgnored
                            ?? attr?.IsIgnored
                            ?? false;

            _Order = Member.GetCustomAttribute<OptionOrderAttribute>()?.Order
                    ?? attr?.Order
                    ?? -1;

            var switchAttr = Member.GetCustomAttribute<SwitchValueAttribute>();
            if (switchAttr != null)
            {
                _HasSwitchValue = true;
                _SwitchValue = switchAttr.SwitchValue;
            }

            var anonymousAttr = Member.GetCustomAttribute<AllowAnonymousAttribute>();

            _AllowAnonymous = anonymousAttr?.AllowAnonymous
                                ?? attr?.AllowAnonymous
                                ?? false;
            _AnonymousPrecedence = anonymousAttr?.AnonymousPrecedence
                                ?? attr?.AnonymousPrecedence
                                ?? -1;
        }

        #endregion

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