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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// <see cref="IgnoreAttribute" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public IgnoreAttribute()
        {
            IsIgnored = true;
        }

        public IgnoreAttribute(bool isIgnored)
        {
            IsIgnored = isIgnored;
        }

        public bool IsIgnored { get; set; }
    }
}