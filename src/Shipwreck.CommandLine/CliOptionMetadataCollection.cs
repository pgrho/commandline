using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    public sealed class CliOptionMetadataCollection : ReadOnlyCollection<CliOptionMetadata>
    {
        internal CliOptionMetadataCollection(IList<CliOptionMetadata> list)
            : base(list)
        {
        }

        public CliOptionMetadata this[string key]
            => key == null ? null
            : this.FirstOrDefault(_ =>
            {
                var m = _.NamesPattern.Match(key);
                return !_.IsIgnored && m.Success && m.Index == 0 && m.Length == key.Length;
            });
    }
}