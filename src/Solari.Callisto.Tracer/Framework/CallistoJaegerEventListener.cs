using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Events;
using OpenTracing;
using OpenTracing.Tag;
using Solari.Callisto.Abstractions;
using Solari.Sol;

namespace Solari.Callisto.Tracer.Framework
{
    public class CallistoJaegerEventListener : ICallistoEventListener
    {
        private readonly IEventFilter _eventFilter;
        private readonly List<string> _events;
        private readonly int _maxPacketSize;
        private readonly ConcurrentDictionary<int, ISpan> _spanCache;
        private readonly ITracer _tracer;

        public CallistoJaegerEventListener(ITracer tracer, IEventFilter eventFilter, IOptions<CallistoTracerOptions> options)
        {
            _tracer = tracer;
            _eventFilter = eventFilter;
            _events = options.Value.TracedDbOperations;
            _maxPacketSize = ConfigureMaxPacketSize(options.Value);
            _spanCache = new ConcurrentDictionary<int, ISpan>();
        }

        public void StartEventHandler(CommandStartedEvent @event)
        {
            if (!_eventFilter.IsOnEventList(@event.CommandName, _events)) return;

            ISpan span = BuildNewSpanWithDefaultTags(@event);
            _spanCache.TryAdd(@event.RequestId, span);
        }

        public void SuccessEventHandler(CommandSucceededEvent @event)
        {
            if (!_eventFilter.IsOnEventList(@event.CommandName, _events)) return;
            if (!_spanCache.TryRemove(@event.RequestId, out ISpan span)) return;
            span.SetTag($"{CallistoConstants.TracerPrefix}.reply.duration.milliseconds", @event.Duration.TotalMilliseconds);
            span.Finish();
        }


        public void ErrorEventHandler(CommandFailedEvent @event)
        {
            if (!_eventFilter.IsOnEventList(@event.CommandName, _events)) return;
            if (!_spanCache.TryRemove(@event.RequestId, out ISpan span)) return;

            span.Log(ExtractExceptionInfo(@event));
            span.SetTag(Tags.Error, true);
            span.Finish();
        }

        private static int ConfigureMaxPacketSize(CallistoTracerOptions options)
        {
            var maxPacketSize = 65000;
            if (options != null && options.MaxPacketSize > 0) maxPacketSize = options.MaxPacketSize;
            return maxPacketSize / 2;
        }

        private static Dictionary<string, object> ExtractExceptionInfo(CommandFailedEvent @event)
        {
            return new Dictionary<string, object>
            {
                {"Event", "ERROR"},
                {"Type", @event.Failure.GetType()},
                {"Message", @event.Failure.Message},
                {"Stack-Trace", @event.Failure.StackTrace}
            };
        }

        private ISpan BuildNewSpanWithDefaultTags(CommandStartedEvent @event)
        {
            IScope activeScope = _tracer.ScopeManager.Active;

            ISpanBuilder span = _tracer
                                .BuildSpan($"{CallistoConstants.TracerPrefix}-{MongoCommandHelper.NormalizeCommandName(@event.CommandName)}")
                                .WithTag(Tags.SpanKind, Tags.SpanKindClient)
                                .WithTag(Tags.Component, "Solari.Callisto")
                                .WithTag(Tags.DbStatement, SanitateDbStatement(@event.Command.ToString()))
                                .WithTag(Tags.DbInstance, @event.DatabaseNamespace.DatabaseName)
                                .WithTag("mongodb.host", @event.ConnectionId.ServerId.EndPoint.ToString())
                                .WithTag("mongodb.host.clusterid", @event.ConnectionId.ServerId.ClusterId.ToString())
                                .WithTag(Tags.DbType, "MongoDb");

            return activeScope?.Span == null ? span.Start() : span.AsChildOf(_tracer.ScopeManager.Active.Span).Start();
        }

        private string SanitateDbStatement(string command)
        {
            return Encoding.UTF8.GetBytes(command).LongLength > _maxPacketSize
                       ? "DbStatement not available because statement size is too big! Consider increasing MaxPacketSize property."
                       : command;
        }
    }
}