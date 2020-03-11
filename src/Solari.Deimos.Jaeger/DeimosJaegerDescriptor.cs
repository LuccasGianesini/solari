using System;
using OpenTracing;
using Solari.Deimos.Abstractions;

namespace Solari.Deimos.Jaeger
{
    public class DeimosJaegerDescriptor : IDisposable
    {
        /// <summary>
        /// The scope of the span
        /// </summary>
        public IScope Scope { get; }

        /// <summary>
        /// The span
        /// </summary>
        public ISpan Span { get; }

        /// <summary>
        /// Indicates if the span should be finished when <see cref="Scope"/> is disposed.
        /// Only need when creating child spans.
        /// </summary>
        public bool FinishOnDispose { get; }

        /// <summary>
        /// The kind of the span.
        /// </summary>
        public SpanKind SpanKind { get; }
        
        /// <summary>
        /// Provide enrichment methods
        /// </summary>
        public ISpanEnricher Enrich { get; }

        private DeimosJaegerDescriptor(IScope scope, ISpan span, bool finishOnDispose, SpanKind spanKind)
        {
            Scope = scope;
            Span = span;
            FinishOnDispose = finishOnDispose;
            SpanKind = spanKind;
            Enrich = new SpanEnricher(Span);
        }

        public void Dispose()
        {
            if (SpanKind == SpanKind.Root)
            {
                Scope?.Dispose();
            }
            else
            {
                if (FinishOnDispose)
                    Span.Finish();
            }
        }
        
        /// <summary>
        /// Create a <see cref="DeimosJaegerDescriptor"/> with <see cref="Abstractions.SpanKind.Root"/>
        /// </summary>
        /// <param name="scope">The scope</param>
        /// <returns><see cref="DeimosJaegerDescriptor"/></returns>
        public static DeimosJaegerDescriptor Create(IScope scope)
            => new DeimosJaegerDescriptor(scope, scope.Span, false, SpanKind.Root);

        /// <summary>
        /// Create a <see cref="DeimosJaegerDescriptor"/> with <see cref="Abstractions.SpanKind.Child"/>
        /// </summary>
        /// <param name="scope">The scope</param>
        /// <param name="span">The child span</param>
        /// <param name="finishOnDispose"></param>
        /// <returns><see cref="DeimosJaegerDescriptor"/></returns>
        public static DeimosJaegerDescriptor Create(IScope scope, ISpan span, bool finishOnDispose)
            => new DeimosJaegerDescriptor(scope, span, finishOnDispose, SpanKind.Child);
    }
}