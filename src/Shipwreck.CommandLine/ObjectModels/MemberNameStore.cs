using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    internal struct MemberNameStore
    {
        private static readonly ReadOnlyCollection<string> EmptyStrings = Array.AsReadOnly(new string[0]);
        private static readonly Regex NeverMatch = new Regex("$.");

        public MemberNameStore(string memberName, ICustomAttributeProvider member, bool exactMatch)
        {
            var memberAttr = member.GetCustomAttribute<MemberAttribute>();
            Name = memberAttr?.Name ?? memberName;

            if (string.IsNullOrEmpty(Name))
            {
                Names = EmptyStrings;
                NamesPattern = NeverMatch;
            }
            else
            {
                var names = member.GetCustomAttributes<CommandLineAliasAttributeAttribute>().Select(_ => _.Alias).OrderBy(_ => _).Distinct(StringComparer.InvariantCultureIgnoreCase).ToList();
                var nv = Name;
                names.RemoveAll(n => nv.Equals(n, StringComparison.InvariantCultureIgnoreCase));
                names.Insert(0, Name);
                Names = new ReadOnlyCollection<string>(names.ToArray());
                NamesPattern = new Regex(
                    (exactMatch ? "^(" : "(")
                        + string.Join("|", Names.Select(Regex.Escape))
                        + (exactMatch ? ")$" : ")"),
                    RegexOptions.IgnoreCase);
            }
        }

        public string Name { get; }

        public ReadOnlyCollection<string> Names { get; }
        public Regex NamesPattern { get; }
    }
}