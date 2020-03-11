using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Events;
using Solari.Callisto.Abstractions;
using Solari.Rhea;

namespace Solari.Callisto.Tracer.Framework
{
    public class CallistoElasticEventListener : ICallistoEventListener
    {
        private readonly IEventFilter _eventFilter;
        private readonly int _maxPacketSize;
        private readonly ConcurrentDictionary<int, ISpan> _spanCache;
        private readonly List<string> _events;
        
        
        public CallistoElasticEventListener(IEventFilter eventFilter, IOptions<CallistoTracerOptions> options)
        {
            _events = options.Value.TracedDbOperations;
            _eventFilter = eventFilter;
            _maxPacketSize = options.Value.MaxPacketSize / 2;
            _spanCache = new ConcurrentDictionary<int, ISpan>();
        }
        private ISpan StartSpan(CommandStartedEvent @event)
        {
            ISpan span =
                Agent.Tracer.CurrentTransaction.StartSpan($"{CallistoConstants.TracerPrefix}-{MongoCommandHelper.NormalizeCommandName(@event.CommandName)}",
                                                          ApiConstants.TypeDb, "mongodb", @event.CommandName);
            span.Labels["component"] = "Solari.Callisto";
            span.Labels["mongodb.host"] = @event.ConnectionId.ServerId.EndPoint.ToString();
            span.Labels["mongodb.host.clusterid"] = @event.ConnectionId.ServerId.ClusterId.ToString();

            span.Context.Db = new Database
            {
                Instance = @event.DatabaseNamespace.DatabaseName,
                Statement = SanitateDbStatement(@event.Command.ToString()),
                Type = "MongoDb"
            };
             
            return span;
        }
        public string SanitateDbStatement(string command)
        {
            return Encoding.UTF8.GetBytes(command).LongLength > _maxPacketSize
                       ? "DbStatement not available because statement size is too big!"
                       : command;
        }


        public void StartEventHandler(CommandStartedEvent @event)
        {
            if (!_eventFilter.IsOnEventList(@event.CommandName, _events)) return;
            ISpan span = StartSpan(@event);
            _spanCache.TryAdd(@event.RequestId, span);
        }

        public void SuccessEventHandler(CommandSucceededEvent @event)
        {
            if (!_eventFilter.IsOnEventList(@event.CommandName, _events)) return;
            if (!_spanCache.TryRemove(@event.RequestId, out ISpan span)) return;
            span.Labels[$"{CallistoConstants.TracerPrefix}.reply.duration.milliseconds"] = @event.Duration.TotalMilliseconds
                                                                                                 .ToString(CultureInfo.InvariantCulture);
            try
            {
                span.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public void ErrorEventHandler(CommandFailedEvent @event)
        {
            if (!_eventFilter.IsOnEventList(@event.CommandName, _events)) return;
            if (!_spanCache.TryRemove(@event.RequestId, out ISpan span)) return;
            span.CaptureException(@event.Failure, @event.Failure.Source);
            span.End();
        }
    }
}