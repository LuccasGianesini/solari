using System;
using System.Collections.Generic;

namespace Solari.Callisto.Framework
{
    public sealed class AppDomainClasses
    {
        public AppDomainClasses(IEnumerable<Type> documentRoots, IEnumerable<Type> documentNodes)
        {
            DocumentNodes = documentNodes;
            DocumentRoots = documentRoots;
        }

        public IEnumerable<Type> DocumentRoots { get; }
        public IEnumerable<Type> DocumentNodes { get; }
    }
}