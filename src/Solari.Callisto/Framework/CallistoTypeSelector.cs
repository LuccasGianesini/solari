using System;
using Solari.Callisto.Abstractions;

namespace Solari.Callisto.Framework
{
    internal static class CallistoTypeSelector
    {
        public static bool IsCallistoDocument(Type type) => IsDocumentRoot(type) || IsDocumentNode(type);
        public static bool IsDocumentRoot(Type type) => typeof(IDocumentRoot).IsAssignableFrom(type);
        public static bool IsDocumentNode(Type type) => typeof(IDocumentNode).IsAssignableFrom(type);
    }
}
