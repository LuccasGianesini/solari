using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using OpenTracing;
using OpenTracing.Propagation;

namespace Solari.Deimos
{
    public static class DeimosJaegerExtensions
    {
        /// <summary>
        /// Extract the headers from a HttpRequest.
        /// </summary>
        /// <param name="requestHeaders">Headers</param>
        /// <returns></returns>
        public static Dictionary<string, string> ExtractHeaders(this IHeaderDictionary requestHeaders)
        {
            return requestHeaders.ToDictionary(k => k.Key, v => v.Value.FirstOrDefault());
        }
        
        /// <summary>
        /// Try to extract the headers from a http span context.
        /// </summary>
        /// <param name="headers">The headers</param>
        /// <param name="tracer">Tracer instance</param>
        /// <param name="outContext">The extracted context</param>
        /// <returns>True if outContext is different then null</returns>
        public static bool TryExtractContext(this IHeaderDictionary headers, ITracer tracer, out ISpanContext outContext)
        {
            outContext = tracer.Extract(BuiltinFormats.HttpHeaders, new TextMapExtractAdapter(headers.ExtractHeaders()));

            return outContext != null;
        }
    }
}