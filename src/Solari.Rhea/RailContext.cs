using System;
using System.Reflection;
using System.Threading;

namespace Solari.Rhea
{
    public abstract class RailContext : IRailContext
    {
        private IContextPayloadCache _payloadCache;

        protected RailContext()
        {
            CancellationToken = CancellationToken.None;
        }

        protected RailContext(params object[] payloads)
        {
            CancellationToken = CancellationToken.None;

            if (payloads != null && payloads.Length > 0)
                _payloadCache = new PayloadCache(payloads);
        }

        protected RailContext(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        protected RailContext(CancellationToken cancellationToken, params object[] payloads)
        {
            CancellationToken = cancellationToken;

            if (payloads != null && payloads.Length > 0)
                _payloadCache = new PayloadCache(payloads);
        }

        public virtual CancellationToken CancellationToken { get; }

        public virtual bool HasPayloadType(Type payloadType)
        {
            return payloadType.GetTypeInfo().IsInstanceOfType(this) || PayloadCache.HasPayloadType(payloadType);
        }

        public virtual bool TryGetPayload<T>(out T payload)
            where T : class
        {
            if (this is T context)
            {
                payload = context;
                return true;
            }

            return PayloadCache.TryGetPayload(out payload);
        }

        public virtual T GetOrAddPayload<T>(PayloadFactory<T> payloadFactory)
            where T : class
        {
            if (this is T context)
                return context;

            return PayloadCache.GetOrAddPayload(payloadFactory);
        }

        public virtual T AddOrUpdatePayload<T>(PayloadFactory<T> addFactory, UpdatePayloadFactory<T> updateFactory)
            where T : class
        {
            if (this is T context)
                return context;

            return PayloadCache.AddOrUpdatePayload(addFactory, updateFactory);
        }

        protected IContextPayloadCache PayloadCache
        {
            get
            {
                if (_payloadCache != null)
                    return _payloadCache;

                while (Volatile.Read(ref _payloadCache) == null)
                    Interlocked.CompareExchange(ref _payloadCache, new PayloadCache(), null);

                return _payloadCache;
            }
        }
    }
}
