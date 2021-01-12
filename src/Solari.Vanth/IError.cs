using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Solari.Vanth
{
    public interface IError : ISerializable
    {
        /// <summary>
        /// The code of the error
        /// </summary>
        string Code { get; init; }
        /// <summary>
        /// The details of the error
        /// </summary>
        List<IErrorDetail> Details { get; }
        /// <summary>
        /// The type of the error. Eg.: database-error, validation-error.
        /// </summary>
        string ErrorType { get; init; }
        /// <summary>
        /// The error message
        /// </summary>
        string Message { get; init; }
        /// <summary>
        /// The target property, method or destination that the error happened
        /// </summary>
        string Target { get; init; }
    }
}
