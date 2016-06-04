using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public sealed class PropertyMetadataCollection : ReadOnlyCollection<PropertyMetadata>
    {
        internal PropertyMetadataCollection(IList<PropertyMetadata> list)
            : base(list)
        {
        }

        public PropertyMetadata this[string key]
            => this.FindByName(key);
    }
}