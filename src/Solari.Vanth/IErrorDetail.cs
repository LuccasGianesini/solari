using System.Runtime.Serialization;

namespace Solari.Vanth
{
    public interface IErrorDetail : ISerializable
    {
        /// <summary>
        /// The code of the error detail
        /// </summary>
        string Code { get; init; }
        /// <summary>
        /// The stack trace of the exception
        /// </summary>
        string StackTrace { get; init; }
        /// <summary>
        /// The help url
        /// </summary>
        string HelpUrl { get; init; }
        /// <summary>
        /// Exception or detail message
        /// </summary>
        string Message { get; init; }
        /// <summary>
        /// The target property, method or destination that the error happened
        /// </summary>
        string Target { get; init; }
        /// <summary>
        /// The source of the error detail
        /// </summary>
        string Source { get; init; }
    }
}

