using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Vanth
{
    public static class ExceptionHelper
    {
        public static IEnumerable<CommonDetailedErrorResponse> ExtractDetailsFromException(this Exception ex)
        {
            if (ex == null)
            {
                yield break;
            }
            yield return new CommonDetailedErrorResponse("", ex.Message, ex.TargetSite?.Name, ex.Source, null);


            IEnumerable<Exception> innerExceptions = Enumerable.Empty<Exception>();

            if (ex is AggregateException exception && exception.InnerExceptions.Any())
            {
                innerExceptions = exception.InnerExceptions;
            }
            else if (ex.InnerException != null)
            {
                innerExceptions = new[] {ex.InnerException};
            }

            foreach (Exception innerEx in innerExceptions)
            {
                yield return new CommonDetailedErrorResponse("", innerEx.Message, innerEx.TargetSite?.Name, innerEx.Source, null);
            }
        }
    }
}