using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracing;

namespace Solari.Themis
{
    public static class ThemisExtensions
    {
        public static void FinalizeTrace(this ISpan spanToFinish, IDictionary<string, object> log)
        {
            if (log is null)
            {
                spanToFinish.Finish();
            }

            spanToFinish.Log(DateTimeOffset.UtcNow, log);
            spanToFinish.Finish();
        }

        public static void FinalizeTrace(this ISpan spanToFinish)
        {
            spanToFinish.Finish();
        }

        public static void FinalizeTrace(this ITracer tracer)
        {
            tracer.ActiveSpan.Finish();
        }

        public static void FinalizeTrace(this ITracer tracer, IDictionary<string, object> log)
        {
            if (log is null)
            {
                tracer.ActiveSpan.Finish();
                return;
            }

            tracer.ActiveSpan.Log(DateTimeOffset.UtcNow, log);
            tracer.ActiveSpan.Finish();
        }
    }
}
