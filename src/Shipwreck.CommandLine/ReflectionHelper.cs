using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shipwreck.CommandLine
{
    internal static class ReflectionHelper
    {
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider)
            => provider.GetCustomAttributes(typeof(T), true).OfType<T>().FirstOrDefault();

        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider provider)
            => provider.GetCustomAttributes(typeof(T), true).OfType<T>().ToArray();
    }
}