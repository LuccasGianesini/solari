using System;
using System.Reflection;

namespace Solari.Sol.Abstractions.Extensions
{
    public static class TypeExtensions
    {
        public static bool CanBeAssignedFrom(this Type type, Type fromType) => type.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
    }
}
