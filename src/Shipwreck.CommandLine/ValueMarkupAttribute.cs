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
    public class ValueMarkupAttribute : Attribute, IMarkupAttribute
    {
        public ValueMarkupAttribute(string value)
        {
            ValueDescription = value;
        }

        public ValueMarkupAttribute(string value, Type resourceType)
        {
            ValueDescription = value;
            ResourceType = resourceType;
        }

        /// <summary>
        /// 値の詳細を表すマークアップの文字列またはリソースキーを取得します。
        /// </summary>
        public string ValueDescription { get; }

        /// <summary>
        /// <see cref="ValueDescription" /> を含むリソースの型を取得します。
        /// </summary>
        public Type ResourceType { get; }

        /// <inheritdoc />
        string IMarkupAttribute.MarkupOrResourceKey => ValueDescription;
    }
}