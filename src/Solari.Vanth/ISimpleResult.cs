using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Solari.Vanth
{
    public interface ISimpleResult: ISerializable
    {
        /// <summary>
        /// The errors of the operation
        /// </summary>
        List<IError> Errors { get; }

        /// <summary>
        /// Indicates if the operation was successful.
        /// <remarks>
        ///  If there are errors in the <see cref="List{T}"/> of <see cref="IError"/>, this property evaluates to false;
        /// </remarks>>
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Add an error to the <see cref="List{T}" of  <see cref="IError"/>/>
        /// </summary>
        /// <param name="error">The error to be added</param>
        /// <returns><see cref="ISimpleResult"/></returns>
        ISimpleResult AddError(IError error);
        /// <summary>
        /// Add an error to the <see cref="List{T}" of  <see cref="IError"/>/>
        /// </summary>
        /// <param name="error">The error to be added</param>
        /// <returns><see cref="ISimpleResult"/></returns>
        ISimpleResult AddError(List<IError> errors);

    }
}
