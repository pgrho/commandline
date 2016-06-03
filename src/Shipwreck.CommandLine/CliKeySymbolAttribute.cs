using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    /// <summary>
    /// クラスを読み込む際に使用するキーの接頭辞を指定します。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CliKeySymbolAttribute : Attribute
    {
        public CliKeySymbolAttribute(string symbol)
        {
            Symbol = symbol;
        }

        public virtual string Symbol { get; }
    }
}