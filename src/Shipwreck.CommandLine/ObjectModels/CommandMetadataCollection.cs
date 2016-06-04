using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public sealed class CommandMetadataCollection : ReadOnlyCollection<CommandMetadata>
    {
        internal CommandMetadataCollection(IList<CommandMetadata> list)
            : base(list)
        {
        }

        public CommandMetadata this[string key]
            => this.FindByName(key);
    }
}