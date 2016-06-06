using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false)]
    public class OptionOrderAttribute : Attribute
    {
        public OptionOrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// メンバーの処理順を取得します。
        /// </summary>
        public virtual int Order { get; }
    }
}