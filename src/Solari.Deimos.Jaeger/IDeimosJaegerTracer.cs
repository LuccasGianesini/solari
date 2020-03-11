﻿﻿using Microsoft.AspNetCore.Http;

  namespace Solari.Deimos.Jaeger
{
    public interface IDeimosJaegerTracer
    {
        /// <summary>
        /// Create a child span and attaches it as a child of the currently active span.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="finishOnDispose">Indicates if the span must be finished when the scope is disposed</param>
        /// <returns><see cref="DeimosJaegerDescriptor"/></returns>
        DeimosJaegerDescriptor StartSpan(string operationName, bool finishOnDispose = true);

        /// <summary>
        /// Opens a new tracing scope and a span.
        /// </summary>
        /// <param name="operationName">The name of the operation</param>
        /// <param name="finishOnDispose">Indicates if the span must be finished when the scope is disposed</param>
        /// <returns><see cref="DeimosJaegerDescriptor"/></returns>
        DeimosJaegerDescriptor StartTransaction(string operationName, bool finishOnDispose = true);

        /// <summary>
        /// Opens a new tracing scope from a http context.
        /// </summary>
        /// <param name="operationName">The name ogf the tracing operation</param>
        /// <param name="httpContext">Http context of the request</param>
        /// <param name="finishOnDispose">Indicates if the span must be finished when the scope is disposed</param>
        /// <returns><see cref="DeimosJaegerDescriptor"/></returns>
        DeimosJaegerDescriptor StartTransaction(string operationName, HttpContext httpContext, bool finishOnDispose = true);
    }
}