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

        public string Description { get; set; }

        public Type DescriptionResourceType { get; set; }

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