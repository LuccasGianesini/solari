using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Solari.Sol.Utils;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    [Serializable]
    public class Error
    {
        public Error(string code, List<ErrorDetail> details, string errorType, object innerError, string message,
                                   string target)
        {
            Code = code;
            Details = details;
            ErrorType = errorType;
            InnerError = innerError;
            Message = message;
            Target = target;
        }

        public Error() { }

        public string Code { get; }
        public List<ErrorDetail> Details { get; } = new List<ErrorDetail>(2);
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
        ///     Adds an <see cref="ErrorDetail" /> into the details list.
        /// </summary>
        /// <param name="detailedError">The detail of error</param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public Error AddDetailedError(ErrorDetail detailedError)
        {
            Details.Add(detailedError);
            return this;
        }

        /// <summary>
        ///     Adds and <see cref="IEnumerable{T}" /> of <see cref="ErrorDetail" /> into the details list.
        ///     It does not clears the list.
        /// </summary>
        /// <param name="detailedErrors"></param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public Error AddDetailedError(IEnumerable<ErrorDetail> detailedErrors)
        {
            Details.AddRange(detailedErrors);
            return this;
        }

        /// <summary>
        ///     Adds an <see cref="ErrorDetail" /> into the details list.
        /// </summary>
        /// <param name="detailedErrors"><see cref="IErrorDetailBuilder" /> delegate</param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public Error AddDetailedError(Func<IErrorDetailBuilder, ErrorDetail> detailedErrors)
        {
            AddDetailedError(detailedErrors(new ErrorDetailBuilder()));
            return this;
        }

        // public override string ToString()
        // {
        //     return
        //         $"Error: {nameof(Code)}: {Code},{Environment.NewLine}{nameof(Message)}: {Message},{Environment.NewLine}{nameof(Target)}: {Target},{Environment.NewLine}{nameof(ErrorType)}: {ErrorType}";
        // }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        ///     Tries to get the list of details.
        /// </summary>
        /// <param name="errorStack">List of <see cref="ErrorDetail" /> wrapped in a <see cref="Maybe{T}" /></param>
        /// <returns>True if the property HasDetails is true. False if it is false</returns>
        public bool TryGetDetails(out Maybe<List<ErrorDetail>> errorStack)
        {
            if (HasDetails)
            {
                errorStack = Maybe<List<ErrorDetail>>.Some(Details);

                return true;
            }

            errorStack = Maybe<List<ErrorDetail>>.None;

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
