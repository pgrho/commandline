using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    internal static class MetadataHelper
    {
        public static T FindByName<T>(this IEnumerable<T> source, string name)
            where T : class, INamedMetadata
        {
            if (name == null)
            {
                return null;
            }

            var nl = name.Length;

            return source.FirstOrDefault(_ =>
            {
                var m = _.NamesPattern.Match(name);
                return !_.IsIgnored && m.Success && m.Index == 0 && m.Length == nl;
            });
        }
    }
}
