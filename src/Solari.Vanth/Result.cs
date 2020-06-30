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
        public Result() { Errors = new Stack<Error>(2); }

        public Stack<Error> Errors { get; private set; }

        public TData Data { get; private set; }

        /// <summary>
        ///     Indicates if the Stack contains any errors in it.
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        ///     Indicates if the result property is different then null.
        ///     It does not check if the property is a primitive, string, DateTime, Timespan, etc. and then checks the value.
        /// </summary>
        public bool HasData => Data != null;


        /// <summary>
        ///     Adds <see cref="Error" /> into the stack.
        /// </summary>
        /// <param name="errorResponse">Error to be added</param>
        /// <returns>
        ///     <see cref="Result{TResult}" />
        /// </returns>
        public Result<TData> AddError(Error errorResponse)
        {
            Errors.Push(errorResponse);
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
        public Result<TData> AddErrors(Stack<Error> errorResponse)
        {
            if (errorResponse == null) throw new ArgumentNullException(nameof(errorResponse));
            foreach (Error commonErrorResponse in errorResponse) Errors.Push(commonErrorResponse);

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
            Errors.Push(builder(new ErrorBuilder()));
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
        ///     Try to get the errors in the stack.
        /// </summary>
        /// <param name="commonErrorResponse">The error stack wrapped in a <see cref="Maybe{T}" /></param>
        /// <returns>True if there is errors in the current stack. False if there is not</returns>
        public bool TryGetErrors(out Maybe<Stack<Error>> commonErrorResponse)
        {
            if (HasErrors)
            {
                commonErrorResponse = Maybe<Stack<Error>>.Some(Errors);

                return true;
            }

            commonErrorResponse = Maybe<Stack<Error>>.None;

            return false;
        }

        /// <summary>
        ///     Tries to get the result of the <see cref="Result{TResult}" />
        /// </summary>
        /// <param name="result">The result wrapped in a <see cref="Maybe{T}" /></param>
        /// <returns>True if the HasResult property is true. False if it is not</returns>
        public bool TryGetResult(out Maybe<TData> result)
        {
            if (HasData)
            {
                result = Maybe<TData>.Some(Data);

                return true;
            }

            result = Maybe<TData>.None;

            return false;
        }

        /// <summary>
        ///     Returns the Errors stack as an <see cref="ImmutableList{T}" />.
        /// </summary>
        /// <returns></returns>
        public IImmutableList<Error> ErrorsAsList() { return Errors.ToImmutableList(); }

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
