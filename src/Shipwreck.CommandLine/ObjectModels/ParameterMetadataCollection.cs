using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    public sealed class ParameterMetadataCollection : ReadOnlyCollection<ParameterMetadata>
    {
        internal ParameterMetadataCollection(IList<ParameterMetadata> list)
            : base(list)
        {
        }

        public ParameterMetadata this[string key]
            => this.FindByName(key);
    }
}