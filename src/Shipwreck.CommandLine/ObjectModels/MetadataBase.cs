using Shipwreck.CommandLine.Markup;
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
    public abstract class MetadataBase
    {
        #region 遅延読み込み状態

        /// <summary>
        /// <see cref="_IsLoaded" />のロックです。
        /// </summary>
        private readonly object _LoadingLock;

        /// <summary>
        /// <see cref="LoadCore" />が実行されたかどうかを示す値です。
        /// </summary>
        private bool _IsLoaded;

        #endregion 遅延読み込み状態

        internal MetadataBase()
        {
            _LoadingLock = new object();
        }

        #region Load

        /// <summary>
        /// 必要であればメタデータを読み込みます。
        /// </summary>
        /// <returns>現在のインスタンス。</returns>
        protected MetadataBase EnsureLoaded()
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
        protected abstract void LoadCore();

        #endregion Load
    }
}