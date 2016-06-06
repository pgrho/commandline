using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine.ObjectModels
{
    internal static class RH
    {
        private static readonly Dictionary<Type, ResourceManager> _Resources = new Dictionary<Type, ResourceManager>();

        private static ResourceManager GetResource(Type resourceType)
        {
            ResourceManager r;
            lock (_Resources)
            {
                if (!_Resources.TryGetValue(resourceType, out r))
                {
                    _Resources[resourceType] = r = new ResourceManager(resourceType);
                }
            }

            return r;
        }

        public static string GetString(string valueOrKey, Type resourceType)
        {
            if (resourceType == null)
            {
                return valueOrKey;
            }
            return GetResource(resourceType).GetString(valueOrKey);
        }
    }
}
