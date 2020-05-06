using System;

namespace Solari.Hyperion.Abstractions
{
    public class HyperionHostedException : Exception
    {
        public HyperionHostedException(string message) : base(message) { }
    }
}