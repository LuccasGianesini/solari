using System;
using System.Collections.Generic;

namespace Solari.Rhea
{
    public class PayloadCache : IContextPayloadCache
    {
        IList<object> _cache;

        public PayloadCache()
        {
        }

        public PayloadCache(object[] payloads)
        {
            _cache = new List<object>(payloads);
        }

        public bool HasPayloadType(Type payloadType)
        {
            lock (this)
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

            lock (this)
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
            lock (this)
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
            lock (this)
            {
                if (_cache != null)
                {
                    for (int i = _cache.Count - 1; i >= 0; i--)
                    {
                        if (_cache[i] is T result)
                        {
                            var updated = updateFactory(result);

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
