using System;
using Microsoft.Extensions.Logging;

namespace Solari.Titan
{
    public interface ITitanLogger<T> where T : class
    {
        ITitanLogger<T> Critical(string message, params object[] args);
        ITitanLogger<T> Critical(string message, Exception exception, params object[] args);
        ITitanLogger<T> Critical(string message, EventId eventId, params object[] args);
        ITitanLogger<T> Critical(string message, Exception exception, EventId eventId, params object[] args);
        ITitanLogger<T> Critical(string message, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Critical(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Critical(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Critical(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Debug(string message, params object[] args);
        ITitanLogger<T> Debug(string message, Exception exception, params object[] args);
        ITitanLogger<T> Debug(string message, EventId eventId, params object[] args);
        ITitanLogger<T> Debug(string message, Exception exception, EventId eventId, params object[] args);
        ITitanLogger<T> Debug(string message, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Debug(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Debug(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Debug(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Error(string message, params object[] args);
        ITitanLogger<T> Error(string message, Exception exception, params object[] args);
        ITitanLogger<T> Error(string message, EventId eventId, params object[] args);
        ITitanLogger<T> Error(string message, Exception exception, EventId eventId, params object[] args);
        ITitanLogger<T> Error(string message, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Error(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Error(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Error(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Information(string message, params object[] args);
        ITitanLogger<T> Information(string message, Exception exception, params object[] args);
        ITitanLogger<T> Information(string message, EventId eventId, params object[] args);
        ITitanLogger<T> Information(string message, Exception exception, EventId eventId, params object[] args);
        ITitanLogger<T> Information(string message, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Information(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Information(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Information(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Trace(string message, params object[] args);
        ITitanLogger<T> Trace(string message, Exception exception, params object[] args);
        ITitanLogger<T> Trace(string message, EventId eventId, params object[] args);
        ITitanLogger<T> Trace(string message, Exception exception, EventId eventId, params object[] args);
        ITitanLogger<T> Trace(string message, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Trace(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Trace(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Trace(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Warning(string message, params object[] args);
        ITitanLogger<T> Warning(string message, Exception exception, params object[] args);
        ITitanLogger<T> Warning(string message, EventId eventId, params object[] args);
        ITitanLogger<T> Warning(string message, Exception exception, EventId eventId, params object[] args);
        ITitanLogger<T> Warning(string message, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Warning(string message, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Warning(string message, EventId eventId, Action<ILogEnricher<T>> erich, params object[] args);
        ITitanLogger<T> Warning(string message, EventId eventId, Exception exception, Action<ILogEnricher<T>> erich, params object[] args);
    }
}