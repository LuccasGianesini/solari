using System;
using Microsoft.Extensions.Logging;
// ReSharper disable ClassNeverInstantiated.Global

namespace Solari.Titan.Framework
{
    public class TitanLogger<T> : ITitanLogger<T> where T : class
    {
        private readonly ILogEnricher<T> _enricher;
        private readonly ILogger<T> _logger;

        public TitanLogger(ILogger<T> logger, ILogEnricher<T> enricher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _enricher = enricher ?? throw new ArgumentNullException(nameof(enricher));
        }

        //------------------------------- Critical ----------------------------------
        public ITitanLogger<T> Critical(string message, params object[] args)
        {
            BaseLogger(LogLevel.Critical, message, 0, args: args);

            return this;
        }

        public ITitanLogger<T> Critical(string message, Exception exception, params object[] args)
        {
            BaseLogger(LogLevel.Critical, message, 0, exception, args);

            return this;
        }

        public ITitanLogger<T> Critical(string message, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Critical, message, eventId, args: args);

            return this;
        }

        public ITitanLogger<T> Critical(string message, Exception exception, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Critical, message, eventId, exception, args);

            return this;
        }

        public ITitanLogger<T> Critical(string message, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Critical, message, 0, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Critical(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Critical, message, 0, erich, exception, args);

            return this;
        }

        public ITitanLogger<T> Critical(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Critical, message, eventId, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Critical(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Critical, message, eventId, erich, exception, args);

            return this;
        }

        //------------------------------- Debug -------------------------------------
        public ITitanLogger<T> Debug(string message, params object[] args)
        {
            BaseLogger(LogLevel.Debug, message, 0, args: args);

            return this;
        }

        public ITitanLogger<T> Debug(string message, Exception exception, params object[] args)
        {
            BaseLogger(LogLevel.Debug, message, 0, exception, args);

            return this;
        }

        public ITitanLogger<T> Debug(string message, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Debug, message, eventId, args: args);

            return this;
        }

        public ITitanLogger<T> Debug(string message, Exception exception, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Debug, message, eventId, exception, args);

            return this;
        }

        public ITitanLogger<T> Debug(string message, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Debug, message, 0, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Debug(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Debug, message, 0, erich, exception, args);

            return this;
        }

        public ITitanLogger<T> Debug(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Debug, message, eventId, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Debug(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Debug, message, eventId, erich, exception, args);

            return this;
        }

        //------------------------------- Error -------------------------------------
        public ITitanLogger<T> Error(string message, params object[] args)
        {
            BaseLogger(LogLevel.Error, message, 0, args: args);

            return this;
        }

        public ITitanLogger<T> Error(string message, Exception exception, params object[] args)
        {
            BaseLogger(LogLevel.Error, message, 0, exception, args);

            return this;
        }

        public ITitanLogger<T> Error(string message, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Error, message, eventId, args: args);

            return this;
        }

        public ITitanLogger<T> Error(string message, Exception exception, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Error, message, eventId, exception, args);

            return this;
        }

        public ITitanLogger<T> Error(string message, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Error, message, 0, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Error(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Error, message, 0, erich, exception, args);

            return this;
        }

        public ITitanLogger<T> Error(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Error, message, eventId, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Error(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Error, message, eventId, erich, exception, args);

            return this;
        }

        //------------------------------- Information -------------------------------
        public ITitanLogger<T> Information(string message, params object[] args)
        {
            BaseLogger(LogLevel.Information, message, 0, args: args);

            return this;
        }

        public ITitanLogger<T> Information(string message, Exception exception, params object[] args)
        {
            BaseLogger(LogLevel.Information, message, 0, exception, args);

            return this;
        }

        public ITitanLogger<T> Information(string message, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Information, message, eventId, args: args);

            return this;
        }

        public ITitanLogger<T> Information(string message, Exception exception, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Information, message, eventId, exception, args);

            return this;
        }

        public ITitanLogger<T> Information(string message, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Information, message, 0, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Information(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Information, message, 0, erich, exception, args);

            return this;
        }

        public ITitanLogger<T> Information(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Information, message, eventId, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Information(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Information, message, eventId, erich, exception, args);

            return this;
        }

        //------------------------------- Trace -------------------------------------
        public ITitanLogger<T> Trace(string message, params object[] args)
        {
            BaseLogger(LogLevel.Trace, message, 0, args: args);

            return this;
        }

        public ITitanLogger<T> Trace(string message, Exception exception, params object[] args)
        {
            BaseLogger(LogLevel.Trace, message, 0, exception, args);

            return this;
        }

        public ITitanLogger<T> Trace(string message, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Trace, message, eventId, args: args);

            return this;
        }

        public ITitanLogger<T> Trace(string message, Exception exception, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Trace, message, eventId, exception, args);

            return this;
        }

        public ITitanLogger<T> Trace(string message, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Trace, message, 0, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Trace(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Trace, message, 0, erich, exception, args);

            return this;
        }

        public ITitanLogger<T> Trace(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Trace, message, eventId, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Trace(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Trace, message, eventId, erich, exception, args);

            return this;
        }

        //------------------------------- Warning -----------------------------------
        public ITitanLogger<T> Warning(string message, params object[] args)
        {
            BaseLogger(LogLevel.Warning, message, 0, args: args);

            return this;
        }

        public ITitanLogger<T> Warning(string message, Exception exception, params object[] args)
        {
            BaseLogger(LogLevel.Warning, message, 0, exception, args);

            return this;
        }

        public ITitanLogger<T> Warning(string message, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Warning, message, eventId, args: args);

            return this;
        }

        public ITitanLogger<T> Warning(string message, Exception exception, EventId eventId, params object[] args)
        {
            BaseLogger(LogLevel.Warning, message, eventId, exception, args);

            return this;
        }

        public ITitanLogger<T> Warning(string message, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Warning, message, 0, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Warning(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Warning, message, 0, erich, exception, args);

            return this;
        }

        public ITitanLogger<T> Warning(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Warning, message, eventId, erich, args: args);

            return this;
        }

        public ITitanLogger<T> Warning(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args)
        {
            BaseEnricher(LogLevel.Warning, message, eventId, erich, exception, args);

            return this;
        }


        //------------------------------- BASE --------------------------------------
        private void BaseEnricher(LogLevel logLevel, string message, EventId eventId, Action<ILogEnricher<T>> erich, Exception exception = null,
                                  params object[] args)
        {
            erich.Invoke(_enricher);
            BaseLogger(logLevel, message, eventId, exception, args);
            _enricher.DisposeScopes();
        }

        private void BaseLogger(LogLevel logLevel, string message, EventId eventId, Exception exception = null, params object[] args)
        {
            _logger.Log(logLevel, eventId, exception, message, args);
        }
    }
}