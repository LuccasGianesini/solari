using System;
using System.Collections.Generic;
using System.Linq;
using Solari.Sol.Utils;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    [Serializable]
    public class CommonErrorResponse
    {
        public CommonErrorResponse(string code, List<CommonDetailedErrorResponse> details, string errorType, object innerError, string message,
                                   string target)
        {
            Code = code;
            Details = details;
            ErrorType = errorType;
            InnerError = innerError;
            Message = message;
            Target = target;
        }

        public CommonErrorResponse() { }

        public string Code { get; }
        public List<CommonDetailedErrorResponse> Details { get; } = new List<CommonDetailedErrorResponse>(2);
        public string ErrorType { get; }
        public object InnerError { get; }
        public string Message { get; }
        public string Target { get; }

        /// <summary>
        ///     Indicates if there is any details in the details list.
        /// </summary>
        public bool HasDetails => Details.Any();


        /// <summary>
        ///     Indicates if the InnerError property is different the null.
        ///     It does not check if the property is a primitive, string, DateTime, Timespan, etc. and then checks the value.
        /// </summary>
        public bool HasInnerError => InnerError != null;

        /// <summary>
        ///     Adds an <see cref="CommonDetailedErrorResponse" /> into the details list.
        /// </summary>
        /// <param name="detailedError">The detail of error</param>
        /// <returns>
        ///     <see cref="CommonErrorResponse" />
        /// </returns>
        public CommonErrorResponse AddDetailedError(CommonDetailedErrorResponse detailedError)
        {
            Details.Add(detailedError);
            return this;
        }

        /// <summary>
        ///     Adds and <see cref="IEnumerable{T}" /> of <see cref="CommonDetailedErrorResponse" /> into the details list.
        ///     It does not clears the list.
        /// </summary>
        /// <param name="detailedErrors"></param>
        /// <returns>
        ///     <see cref="CommonErrorResponse" />
        /// </returns>
        public CommonErrorResponse AddDetailedError(IEnumerable<CommonDetailedErrorResponse> detailedErrors)
        {
            Details.AddRange(detailedErrors);
            return this;
        }

        /// <summary>
        ///     Adds an <see cref="CommonDetailedErrorResponse" /> into the details list.
        /// </summary>
        /// <param name="detailedErrors"><see cref="ICommonDetailedErrorResponseBuilder" /> delegate</param>
        /// <returns>
        ///     <see cref="CommonErrorResponse" />
        /// </returns>
        public CommonErrorResponse AddDetailedError(Func<ICommonDetailedErrorResponseBuilder, CommonDetailedErrorResponse> detailedErrors)
        {
            AddDetailedError(detailedErrors(new CommonDetailedErrorResponseBuilder()));
            return this;
        }

        public override string ToString()
        {
            return
                $"Error: {nameof(Code)}: {Code},{Environment.NewLine}{nameof(Message)}: {Message},{Environment.NewLine}{nameof(Target)}: {Target},{Environment.NewLine}{nameof(ErrorType)}: {ErrorType}";
        }

        /// <summary>
        ///     Tries to get the list of details.
        /// </summary>
        /// <param name="errorStack">List of <see cref="CommonDetailedErrorResponse" /> wrapped in a <see cref="Maybe{T}" /></param>
        /// <returns>True if the property HasDetails is true. False if it is false</returns>
        public bool TryGetDetails(out Maybe<List<CommonDetailedErrorResponse>> errorStack)
        {
            if (HasDetails)
            {
                errorStack = Maybe<List<CommonDetailedErrorResponse>>.Some(Details);

                return true;
            }

            errorStack = Maybe<List<CommonDetailedErrorResponse>>.None;

            return false;
        }

        /// <summary>
        ///     Tries to get the inner error.
        /// </summary>
        /// <param name="innerError">Property InnerError wrapped in a <see cref="Maybe{T}" /></param>
        /// <returns>True if the property HasInnerError is true. False if it is false</returns>
        public bool TryGetInnerError(out Maybe<object> innerError)
        {
            if (HasInnerError)
            {
                innerError = Maybe<object>.Some(InnerError);

                return true;
            }

            innerError = Maybe<object>.None;

            return false;
        }

        /// <summary>
        ///     Clear the details list.
        /// </summary>
        public void ClearDetails()
        {
            if (Details.Any())
                Details.Clear();
        }
    }
}