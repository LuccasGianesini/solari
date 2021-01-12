using System.Collections.Generic;
using System.Runtime.Serialization;
using Solari.Sol.Abstractions;

namespace Solari.Vanth
{
    public interface IResult<T> : ISerializable
    {
        /// <summary>
        /// Data of the result
        /// </summary>
        T Data { get; init; }
        /// <summary>
        ///  The errors of the operation
        /// </summary>
        List<IError> Errors { get; }
        /// <summary>
        /// Indicates if the operation was successful.
        /// <remarks>
        ///  If there are errors in the <see cref="List{T}"/> of <see cref="IError"/>, this property evaluates to false;
        ///  If <see cref="Data"/> is different then null, this property evaluates to true;
        /// </remarks>
        /// </summary>
        bool Success { get; }
        /// <summary>
        /// Add an error to the <see cref="List{T}" of  <see cref="IError"/>/>
        /// </summary>
        /// <param name="error">The error to be added</param>
        /// <returns><see cref="IResult{T}"/></returns>
        IResult<T> AddError(IError error);
        /// <summary>
        /// Add an error to the <see cref="List{T}" of  <see cref="IError"/>/>
        /// </summary>
        /// <param name="error">The error to be added</param>
        /// <returns><see cref="IResult{T}"/></returns>
        IResult<T> AddError(List<IError> errors);
    }
}
