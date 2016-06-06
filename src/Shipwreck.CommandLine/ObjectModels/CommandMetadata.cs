using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public abstract class CommandMetadata : LoaderMetadata, ICommandMetadata, INamedMetadata
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

        /// <summary>
        /// <see cref="IsIgnored" />のバッキングストアです。
        /// </summary>
        private bool _IsIgnored;

        /// <summary>
        /// <see cref="Order" />のバッキングストアです。
        /// </summary>
        private int _Order;

        #endregion

        internal CommandMetadata(MemberInfo member)
        {
            _NameStore = new MemberNameStore(member.Name, member, true);

            _LoadingLock = new object();
        }


        public string Name => _NameStore.Name;

        public ReadOnlyCollection<string> Names => _NameStore.Names;

        public Regex NamesPattern => _NameStore.NamesPattern;


        protected MemberInfo Member => (MemberInfo)_NameStore.Member;

        #region Load

        /// <summary>
        /// 必要であればメタデータを読み込みます。
        /// </summary>
        /// <returns>現在のインスタンス。</returns>
        protected CommandMetadata EnsureLoaded()
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
            var attr = Member.GetCustomAttribute<OptionAttribute>();

            _IsIgnored = Member.GetCustomAttribute<IgnoreAttribute>()?.IsIgnored
                            ?? attr?.IsIgnored
                            ?? false;

            _Order = Member.GetCustomAttribute<OptionOrderAttribute>()?.Order
                    ?? attr?.Order
                    ?? -1;
        }

        #endregion

        #region 遅延読み込みプロパティ
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

        #endregion


        public abstract CommandMetadataCollection Commands { get; }

        internal abstract LoadingContextBase CreateContextForDeclaringObject(CommandMetadata metadata, LoaderSettings settings, IEnumerable<string> args, LoadingContextBase parentContext);

        object ICommandMetadata.ExecuteCore(LoadingContextBase context, object parameter)
            => ExecuteCore(context, parameter);

        internal abstract object ExecuteCore(LoadingContextBase context, object parameter);
    }
}