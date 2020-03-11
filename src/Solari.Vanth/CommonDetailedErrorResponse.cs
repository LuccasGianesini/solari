using System;
using System.Text;
using Solari.Rhea;

namespace Solari.Vanth
{
    [Serializable]
    public class CommonDetailedErrorResponse
    {
        public CommonDetailedErrorResponse(string code, string message, string target, Exception exception)
        {
            Code = code;
            Message = message;
            Target = target;
            Exception = exception;
        }

        public string Code { get; }
        public Exception Exception { get; }
        public string Message { get; }
        public string Target { get; }

        /// <summary>
        /// Indicates if the Exception is different the null.
        /// </summary>
        public bool HasException => Exception != null;


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder()
                               .Append($"Detail:").AppendLine()
                               .Append($"{nameof(Code)}: {Code}").AppendLine()
                               .Append($"{nameof(Message)}: {Message}").AppendLine()
                               .Append($"{nameof(Target)}: {Target}").AppendLine();
            if (Exception != null)
            {
                sb.Append("Exception:").AppendLine()
                  .Append($"Exception Message: {Exception.Message}")
                  .Append($"Source: {Exception.Source}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Tries to get the Exception.
        /// </summary>
        /// <param name="mayBeException">The exception wrapped in a <see cref="Maybe{T}"/></param>
        /// <returns>True if HasException is true. False if it is false</returns>
        public bool TryGetException(out Maybe<Exception> mayBeException)
        {
            if (HasException)
            {
                mayBeException = Maybe<Exception>.Some(Exception);

                return true;
            }

            mayBeException = Maybe<Exception>.None;

            return false;
        }

    }
}