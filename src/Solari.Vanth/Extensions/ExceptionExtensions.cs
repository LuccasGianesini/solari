using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Vanth.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<ErrorDetail> ExtractDetailsFromException(this Exception ex, bool addStackTrace)
        {
            if (ex is null) yield break;
            yield return new ErrorDetail
            {
                Message = ex.Message,
                Target = ex.TargetSite?.Name,
                Source = ex.Source,
                StackTrace = addStackTrace? ex.StackTrace : "",
                HelpUrl = ex.HelpLink
            };


            IEnumerable<Exception> innerExceptions = Enumerable.Empty<Exception>();

            if (ex is AggregateException exception && exception.InnerExceptions.Any())
                innerExceptions = exception.InnerExceptions;
            else if (ex.InnerException != null) innerExceptions = new[] {ex.InnerException};

            foreach (Exception innerEx in innerExceptions)
                yield return new ErrorDetail
                {
                    Message = innerEx.Message,
                    Target = innerEx.TargetSite?.Name,
                    Source = innerEx.Source,
                    StackTrace = addStackTrace? ex.StackTrace : "",
                    HelpUrl = innerEx.HelpLink
                };
        }
    }
}
