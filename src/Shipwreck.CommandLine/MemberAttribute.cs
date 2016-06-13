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
    public abstract class MemberAttribute : Attribute
    {
        public string Name { get; set; }

        #region Description

        /// <summary>
        /// メンバーを表すマークアップの文字列またはリソースキーを取得または設定します。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// <see cref="Description" /> を含むリソースの型を取得または設定します。
        /// </summary>
        public Type DescriptionResourceType { get; set; }

        #endregion Description

        /// <summary>
        /// メンバーが無視されるかどうかを示す値を取得または設定します。
        /// </summary>
        public bool IsIgnored { get; set; }

        /// <summary>
        /// メンバーの処理順を取得または設定します。
        /// </summary>
        public int Order { get; set; }
    }
}