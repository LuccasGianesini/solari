using System.Collections.Generic;
using OpenTracing;

namespace Solari.Themis
{
    public static class ThemisExtensions
    {
        public static void FinalizeTrace(this ISpan spanToFinish, IDictionary<string, object> log)
        {
            if (log != null) spanToFinish.Log(log);
            spanToFinish.Finish();
        }
        
        public static void FinalizeTrace(this ISpan spanToFinish)
        {
            spanToFinish.Finish();
        }
    }
}