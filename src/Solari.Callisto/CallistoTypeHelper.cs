using System;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto
{
    internal static class CallistoTypeHelper
    {
        public static bool IsCallistoClass(Type type) => IsDocumentRoot(type) || IsDocumentNode(type);
        public static bool IsDocumentRoot(Type type) => typeof(IDocumentRoot).IsAssignableFrom(type);
        public static bool IsDocumentNode(Type type) => typeof(IDocumentNode).IsAssignableFrom(type);
    }
}