using System;

namespace Solari.Ganymede.Domain.Exceptions
{
    public class CommandNotAvailableException : Exception
    {
        public CommandNotAvailableException() : base("Cannot find command in registry") { }

        public CommandNotAvailableException(string message)
            : base(message)
        {
        }

        public CommandNotAvailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}