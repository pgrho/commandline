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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Method, AllowMultiple = false)]
    public class DescriptionMarkupAttribute : Attribute, IMarkupAttribute
    {
        public DescriptionMarkupAttribute(string description)
        {
            Description = description;
        }

        public DescriptionMarkupAttribute(string description, Type resourceType)
        {
            Description = description;
            ResourceType = resourceType;
        }

        /// <summary>
        /// メンバーを表すマークアップの文字列またはリソースキーを取得します。
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// <see cref="Description" />を含むリソースの型を取得します。
        /// </summary>
        public Type ResourceType { get; }

        /// <inheritdoc />
        string IMarkupAttribute.MarkupOrResourceKey => Description;
    }
}