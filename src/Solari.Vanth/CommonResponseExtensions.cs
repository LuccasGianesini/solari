using System;
using System.Collections.Generic;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public static class CommonResponseExtensions
    {
        /// <summary>
        /// Creates a new <see cref="CommonResponse{TModel}"/> with a different generic type. And add the new generic type value. 
        /// </summary>
        /// <param name="commonResponse">CommonResponse object to be cloned.</param>
        /// <param name="newTypeValue">The new generic type value. Value must not be null.</param>
        /// <param name="addErrors">Indicates if the errors present in the old CommonResponse must be added into the new CommonResponse object</param>
        /// <typeparam name="TNewGenericType">The new generic type</typeparam>
        /// <typeparam name="TOldGenericType">The old generic Type</typeparam>
        /// <returns>The <see cref="CommonResponse{TModel}"/> complete with errors and model in the new generic type</returns>
        /// <exception cref="ArgumentNullException">When commonResponse is null</exception>
        public static CommonResponse<TNewGenericType> ToNewGenericType<TNewGenericType, TOldGenericType>
            (this CommonResponse<TOldGenericType> commonResponse, TNewGenericType newTypeValue, bool addErrors)
        {
            if (commonResponse == null) throw new ArgumentNullException(nameof(commonResponse));
            
            ICommonResponseBuilder<TNewGenericType> builder = new CommonResponseBuilder<TNewGenericType>().WithResult(newTypeValue);
            if (commonResponse.HasErrors && addErrors)
            {
                builder.WithErrors(commonResponse.Errors);
            }
            return builder.Build();
        }

        /// <summary>
        /// Creates a new <see cref="CommonResponse{TModel}"/> with a different generic type. 
        /// </summary>
        /// <param name="commonResponse">CommonResponse object to be cloned.</param>
        /// <typeparam name="TNewGenericType">The new generic type</typeparam>
        /// <typeparam name="TOldGenericType">The old generic Type</typeparam>
        /// <returns>The <see cref="CommonResponse{TModel}"/> complete with only the errors</returns>
        /// <exception cref="ArgumentNullException">When commonResponse is null</exception>
        public static CommonResponse<TNewGenericType> ToNewGenericTypeWithErrorsOnly<TNewGenericType, TOldGenericType>
        (this CommonResponse<TOldGenericType> commonResponse)
        {
            if (commonResponse == null) throw new ArgumentNullException(nameof(commonResponse));
            return new CommonResponseBuilder<TNewGenericType>().WithErrors(commonResponse.Errors).Build();
        }
        
    }
}