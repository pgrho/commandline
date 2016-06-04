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
    public sealed class CliLoadingSettings
    {
        private static readonly ReadOnlyCollection<string> DefaultKeySymbols = new ReadOnlyCollection<string>(new[] { "-", "--", "/" });
        private static readonly Regex DefaultKeyPattern = new Regex("^(--?|/)");
        private static readonly ReadOnlyCollection<string> DefaultAssignmentSymbols = new ReadOnlyCollection<string>(new[] { "=", ":" });
        private static readonly Regex DefaultAssignmentPattern = new Regex("(:|=)");

        public CliLoadingSettings(Type type)
            : this(GetArgumentStyle(type), GetKeySymbols(type), GetAssignmentSymbols(type))
        { }


        public CliLoadingSettings(ArgumentStyle argumentStyle, IEnumerable<string> keySymbols, IEnumerable<string> assignmentSymbols)
        {
            ArgumentStyle = argumentStyle;
            KeySymbols = keySymbols == null ? DefaultKeySymbols
                            : Array.AsReadOnly(keySymbols.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray());
            KeyPattern = KeySymbols == DefaultKeySymbols ? DefaultKeyPattern
                            : new Regex("^(" + string.Join("|", KeySymbols.Select(Regex.Escape)) + ")");
            AssignmentSymbols = assignmentSymbols == null ? DefaultAssignmentSymbols
                            : Array.AsReadOnly(assignmentSymbols.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray());
            AssignmentPattern = AssignmentSymbols == DefaultKeySymbols ? DefaultAssignmentPattern
                            : new Regex("(" + string.Join("|", AssignmentSymbols.Select(Regex.Escape)) + ")");
        }

        public ArgumentStyle ArgumentStyle { get; }

        public ReadOnlyCollection<string> KeySymbols { get; }

        public Regex KeyPattern { get; }

        public ReadOnlyCollection<string> AssignmentSymbols { get; }

        public Regex AssignmentPattern { get; }

   
        // public bool HasIndependentValue { get; set; }
        //public Regex NewInstancePattern { get; set; }

        //public Regex ClearItemPattern { get; set; }
        //public Regex AddItemPattern { get; set; }
        //public Regex RemoveItemPattern { get; set; }
        //public Regex SetItemPattern { get; set; }

        private static ArgumentStyle GetArgumentStyle(Type type)
            => type.GetCustomAttribute<ArgumentStyleAttribute>()?.ArgumentStyle ?? ArgumentStyle.Combined;

        private static IEnumerable<string> GetKeySymbols(Type type)
        {
            var ks = type.GetCustomAttributes<CliKeySymbolAttribute>().Select(_ => _.Symbol).ToArray();
            return ks.Any() ? ks : null;
        }
        private static IEnumerable<string> GetAssignmentSymbols(Type type)
        {
            var ks = type.GetCustomAttributes<AssignmentSymbolAttribute>().Select(_ => _.Symbol).ToArray();
            return ks.Any() ? ks : null;
        }
    }
}