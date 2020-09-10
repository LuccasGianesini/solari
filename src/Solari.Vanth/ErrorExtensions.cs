using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public static class ErrorExtensions
    {
        /// <summary>
        ///     Adds an <see cref="ErrorDetail" /> into the details list.
        /// </summary>
        /// <param name="detailedError">The detail of error</param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public static Error AddErrorDetail(this Error error, ErrorDetail detailedError)
        {
            error.Details.Add(detailedError);
            return error;
        }

        /// <summary>
        ///     Adds and <see cref="IEnumerable{T}" /> of <see cref="ErrorDetail" /> into the details list.
        ///     It does not clears the list.
        /// </summary>
        /// <param name="detailedErrors"></param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public static Error AddErrorDetail(this Error error, IEnumerable<ErrorDetail> detailedErrors)
        {
            error.Details.AddRange(detailedErrors);
            return error;
        }

        /// <summary>
        ///     Adds an <see cref="ErrorDetail" /> into the details list.
        /// </summary>
        /// <param name="detailedErrors"><see cref="IErrorDetailBuilder" /> delegate</param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public static Error AddErrorDetail(this Error error, Func<IErrorDetailBuilder, ErrorDetail> detailedErrors)
        {
            error.AddErrorDetail(detailedErrors(new ErrorDetailBuilder()));
            return error;
        }


        public static string ToString(this Error error)
        {
            return JsonConvert.SerializeObject(error);
        }

        /// <summary>
        ///     Clear the details list.
        /// </summary>
        public static void ClearDetails(this Error error)
        {
            if (error.Details.Any())
                error.Details.Clear();
        }
    }
}
