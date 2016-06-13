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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class OptionAttribute : MemberAttribute
    {
        /// <summary>
        /// プロパティが無名オプションとなりうるかどうかを示す値を取得または設定します。
        /// </summary>
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// 無名オプション内のプロパティの優先順位を取得または設定します。
        /// </summary>
        public int AnonymousPrecedence { get; set; }

        #region ValueDescription

        /// <summary>
        /// 値の詳細を表すマークアップの文字列またはリソースキーを取得または設定します。
        /// </summary>
        public string ValueDescription { get; set; }

        /// <summary>
        /// <see cref="ValueDescription" /> を含むリソースの型を取得または設定します。
        /// </summary>
        public Type ValueDescriptionResourceType { get; set; }

        #endregion ValueDescription
    }
}