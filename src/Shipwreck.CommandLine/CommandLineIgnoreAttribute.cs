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
    public class CommandLineIgnoreAttribute : Attribute
    {
        /// <summary>
        /// <see cref="CommandLineIgnoreAttribute" />クラスの新しいインスタンスを初期化します。
        /// </summary>
        public CommandLineIgnoreAttribute()
        {
            IsIgnored = true;
        }

        public CommandLineIgnoreAttribute(bool isIgnored)
        {
            IsIgnored = isIgnored;
        }

        public bool IsIgnored { get; set; }
    }
}