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

        public CliLoadingSettings(IEnumerable<string> keySymbols, IEnumerable<string> assignmentSymbols)
        {
            KeySymbols = keySymbols == null ? DefaultKeySymbols
                            : Array.AsReadOnly(keySymbols.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray());
            KeyPattern = KeySymbols == DefaultKeySymbols ? DefaultKeyPattern
                            : new Regex("^(" + string.Join("|", KeySymbols.Select(Regex.Escape)) + ")");
            AssignmentSymbols = assignmentSymbols == null ? DefaultAssignmentSymbols
                            : Array.AsReadOnly(assignmentSymbols.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray());
            AssignmentPattern = AssignmentSymbols == DefaultKeySymbols ? DefaultAssignmentPattern
                            : new Regex("(" + string.Join("|", AssignmentSymbols.Select(Regex.Escape)) + ")");
        }

        public ReadOnlyCollection<string> KeySymbols { get; }

        public Regex KeyPattern { get; }

        public ReadOnlyCollection<string> AssignmentSymbols { get; }

        public Regex AssignmentPattern { get; }

        public CliOptionLoader CreateLoader(CliTypeMetadata metadata, CliLoadingSettings settings, object target, IEnumerable<string> args) 
            => new CliOptionLoader(metadata, settings, target, args);


        // public bool HasIndependentValue { get; set; }
        //public Regex NewInstancePattern { get; set; }

        //public Regex ClearItemPattern { get; set; }
        //public Regex AddItemPattern { get; set; }
        //public Regex RemoveItemPattern { get; set; }
        //public Regex SetItemPattern { get; set; }
    }
}