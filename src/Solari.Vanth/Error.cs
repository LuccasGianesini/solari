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

        public string Code { get; set; }
        public List<ErrorDetail> Details { get; set;} = new List<ErrorDetail>(2);
        public string ErrorType { get; set;}
        public string Message { get; set;}
        public string Target { get; set;}

        /// <summary>
        ///     Adds an <see cref="ErrorDetail" /> into the details list.
        /// </summary>
        /// <param name="detailedError">The detail of error</param>
        /// <returns>
        ///     <see cref="Error" />
        /// </returns>
        public Error AddErrorDetail(ErrorDetail detailedError)
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
        public Error AddErrorDetail(IEnumerable<ErrorDetail> detailedErrors)
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
        public Error AddErrorDetail(Func<IErrorDetailBuilder, ErrorDetail> detailedErrors)
        {
            AddErrorDetail(detailedErrors(new ErrorDetailBuilder()));
            return this;
        }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
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
