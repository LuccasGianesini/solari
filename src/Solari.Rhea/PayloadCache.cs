using System;
using System.Collections.Generic;

namespace Solari.Rhea
{
    public class PayloadCache : IContextPayloadCache
    {
        IList<object> _cache;
        private readonly object _synLock;

        public PayloadCache()
        {
            _synLock = new object();
        }

        public PayloadCache(object[] payloads)
        {
            _cache = new List<object>(payloads);
            _synLock = new object();
        }

        public bool HasPayloadType(Type payloadType)
        {
            lock (_synLock)
            {
                if (_cache == null)
                    return false;

                for (int i = _cache.Count - 1; i >= 0; i--)
                {
                    if (payloadType.IsInstanceOfType(_cache[i]))
                        return true;
                }
            }

            return false;
        }

        public bool TryGetPayload<T>(out T payload)
            where T : class
        {
            if (_cache == null)
            {
                payload = default;
                return false;
            }

            lock (_synLock)
            {
                for (int i = _cache.Count - 1; i >= 0; i--)
                {
                    if (_cache[i] is T p)
                    {
                        payload = p;
                        return true;
                    }
                }
            }

            payload = default;
            return false;
        }

        public T GetOrAddPayload<T>(PayloadFactory<T> payloadFactory)
            where T : class
        {
            lock (_synLock)
            {
                if (_cache != null)
                {
                    for (int i = _cache.Count - 1; i >= 0; i--)
                    {
                        if (_cache[i] is T result)
                            return result;
                    }
                }

                T payload = payloadFactory();

                if (_cache != null)
                    _cache.Add(payload);
                else
                    _cache = new List<object>(1) {payload};

                return payload;
            }
        }

        public T AddOrUpdatePayload<T>(PayloadFactory<T> addFactory, UpdatePayloadFactory<T> updateFactory)
            where T : class
        {
            lock (_synLock)
            {
                if (_cache != null)
                {
                    for (int i = _cache.Count - 1; i >= 0; i--)
                    {
                        if (_cache[i] is T result)
                        {
                            T updated = updateFactory(result);

                            _cache[i] = updated;

                            return updated;
                        }
                    }
                }

                T payload = addFactory();

                if (_cache != null)
                    _cache.Add(payload);
                else
                    _cache = new List<object>(1) {payload};

                return payload;
            }
        }
    }
}
