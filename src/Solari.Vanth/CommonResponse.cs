using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using Solari.Sol.Utils;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    [Serializable]
    public class CommonResponse<TResult>
    {
        
        public CommonResponse()
        {
            Errors = new Stack<CommonErrorResponse>(2);
        }

        public Stack<CommonErrorResponse> Errors { get; private set; }
        
        public TResult Result { get; private set; }

        /// <summary>
        /// Indicates if the Stack contains any errors in it.
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// Indicates if the result property is different then null.
        /// It does not check if the property is a primitive, string, DateTime, Timespan, etc. and then checks the value.
        /// </summary>
        public bool HasResult => Result != null;
        

        /// <summary>
        /// Adds <see cref="CommonErrorResponse"/> into the stack.
        /// </summary>
        /// <param name="errorResponse">Error to be added</param>
        /// <returns><see cref="CommonResponse{TResult}"/></returns>
        public CommonResponse<TResult> AddError(CommonErrorResponse errorResponse)
        {
            Errors.Push(errorResponse);
            return this;
        }

        /// <summary>
        /// Adds a stack of <see cref="CommonErrorResponse"/> into the current stack.
        /// It does not clear the current error stack before adding.
        /// </summary>
        /// <param name="errorResponse">Error to be added</param>
        /// <returns><see cref="CommonResponse{TResult}"/></returns>
        public CommonResponse<TResult> AddErrors(Stack<CommonErrorResponse> errorResponse)
        {
            if (errorResponse == null) throw new ArgumentNullException(nameof(errorResponse));
            foreach (CommonErrorResponse commonErrorResponse in errorResponse)
            {
                Errors.Push(commonErrorResponse);
            }

            return this;
        }

        /// <summary>
        /// Adds an <see cref="CommonErrorResponse"/> into the stack.
        /// </summary>
        /// <param name="builder"><see cref="ICommonErrorResponseBuilder"/> delegate</param>
        /// <returns><see cref="CommonResponse{TResult}"/></returns>
        public CommonResponse<TResult> AddError(Func<ICommonErrorResponseBuilder, CommonErrorResponse> builder)
        {
            Errors.Push(builder(new CommonErrorResponseBuilder()));
            return this;
        }
        
        /// <summary>
        /// Adds an result.
        /// </summary>
        /// <param name="result">The result</param>
        /// <returns><see cref="CommonResponse{TResult}"/></returns>
        public CommonResponse<TResult> AddResult(TResult result)
        {
            Result = result;
            return this;
        }

        /// <summary>
        /// Try to get the errors in the stack.
        /// </summary>
        /// <param name="commonErrorResponse">The error stack wrapped in a <see cref="Maybe{T}"/></param>
        /// <returns>True if there is errors in the current stack. False if there is not</returns>
        public bool TryGetErrors(out Maybe<Stack<CommonErrorResponse>> commonErrorResponse)
        {
            if (HasErrors)
            {
                commonErrorResponse = Maybe<Stack<CommonErrorResponse>>.Some(Errors);

                return true;
            }

            commonErrorResponse = Maybe<Stack<CommonErrorResponse>>.None;

            return false;
        }

        /// <summary>
        /// Tries to get the result of the <see cref="CommonResponse{TResult}"/>
        /// </summary>
        /// <param name="result">The result wrapped in a <see cref="Maybe{T}"/></param>
        /// <returns>True if the HasResult property is true. False if it is not</returns>
        public bool TryGetResult(out Maybe<TResult> result)
        {
            if (HasResult)
            {
                result = Maybe<TResult>.Some(Result);

                return true;
            }

            result = Maybe<TResult>.None;

            return false;
        }

        /// <summary>
        /// Returns the Errors stack as an <see cref="ImmutableList{T}"/>.
        /// </summary>
        /// <returns></returns>
        public IImmutableList<CommonErrorResponse> ErrorsAsList() => Errors.ToImmutableList();

        /// <summary>
        /// Clear the error stack. This method also clear the details o each error in the stack.
        /// </summary>
        public void ClearErrors()
        {
            if (!Errors.Any()) return;
            foreach (CommonErrorResponse commonErrorResponse in Errors)
            {
                commonErrorResponse.ClearDetails();
            }
            Errors.Clear();

        }
    }
    
}