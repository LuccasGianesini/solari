using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;
using Solari.Sol.Utils;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    [Serializable]
    public class Result<TData>
    {
        public Result()
        {
            Errors = new List<Error>(2);
        }

        public List<Error> Errors { get; set; }

        public TData Data { get; set; }

        /// <summary>
        ///     Adds <see cref="Error" /> into the stack.
        /// </summary>
        /// <param name="errorResponse">Error to be added</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public Result<TData> AddError(Error errorResponse)
        {
            Errors.Add(errorResponse);
            return this;
        }

        /// <summary>
        ///     Adds a stack of <see cref="Error" /> into the current stack.
        ///     It does not clear the current error stack before adding.
        /// </summary>
        /// <param name="errorResponse">Error to be added</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public Result<TData> AddErrors(List<Error> errorResponse)
        {
            if (errorResponse == null) throw new ArgumentNullException(nameof(errorResponse));
            foreach (Error commonErrorResponse in errorResponse) Errors.Add(commonErrorResponse);

            return this;
        }

        /// <summary>
        ///     Adds an <see cref="Error" /> into the stack.
        /// </summary>
        /// <param name="builder"><see cref="IErrorBuilder" /> delegate</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public Result<TData> AddError(Func<IErrorBuilder, Error> builder)
        {
            Errors.Add(builder(new ErrorBuilder()));
            return this;
        }

        /// <summary>
        ///     Adds an result.
        /// </summary>
        /// <param name="result">The result</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public Result<TData> AddResult(TData result)
        {
            Data = result;
            return this;
        }

        /// <summary>
        ///     Clear the error stack. This method also clear the details o each error in the stack.
        /// </summary>
        public void ClearErrors()
        {
            if (!Errors.Any()) return;
            foreach (Error commonErrorResponse in Errors) commonErrorResponse.ClearDetails();
            Errors.Clear();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
