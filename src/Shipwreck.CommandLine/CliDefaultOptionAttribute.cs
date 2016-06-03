using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    /// <summary>
    /// プロパティが無名オプションとなりうるかどうかを指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CliDefaultOptionAttribute : Attribute
    {
        public CliDefaultOptionAttribute()
        {
            IsDefault = true;
        }

        public CliDefaultOptionAttribute(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public CliDefaultOptionAttribute(int precedence)
        {
            IsDefault = true;
            Precedence = precedence;
        }

        public CliDefaultOptionAttribute(bool isDefault, int precedence)
        {
            IsDefault = isDefault;
            Precedence = precedence;
        }

        /// <summary>
        /// プロパティが無名オプションとなりうるかどうかを示す値を取得します。
        /// </summary>
        public virtual bool IsDefault { get; }

        /// <summary>
        /// 無名オプション内のプロパティの優先順位を取得します。
        /// </summary>
        public virtual int Precedence { get; }
    }
}