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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AllowAnonymousAttribute : Attribute
    {
        public AllowAnonymousAttribute()
        {
            AllowAnonymous = true;
        }

        public AllowAnonymousAttribute(bool allowAnonymous)
        {
            AllowAnonymous = allowAnonymous;
        }

        public AllowAnonymousAttribute(int anonymousPrecedence)
        {
            AllowAnonymous = true;
            AnonymousPrecedence = anonymousPrecedence;
        }

        public AllowAnonymousAttribute(bool allowAnonymous, int anonymousPrecedence)
        {
            AllowAnonymous = allowAnonymous;
            AnonymousPrecedence = anonymousPrecedence;
        }

        /// <summary>
        /// プロパティが無名オプションとなりうるかどうかを示す値を取得します。
        /// </summary>
        public virtual bool AllowAnonymous { get; }

        /// <summary>
        /// 無名オプション内のプロパティの優先順位を取得します。
        /// </summary>
        public virtual int AnonymousPrecedence { get; }
    }
}