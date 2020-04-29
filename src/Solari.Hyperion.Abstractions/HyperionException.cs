using System;

namespace Solari.Hyperion.Abstractions
{
    public class HyperionException : Exception
    {
        public HyperionException(string message) : base(message) { }
    }
}