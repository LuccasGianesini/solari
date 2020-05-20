using System;

namespace Solari.Juno.Abstractions
{
    internal class JunoAuthException : Exception
    {
        public JunoAuthException(string message) : base(message) { }
    }
}