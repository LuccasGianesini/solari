using System;

namespace Solari.Rhea
{
    public interface IContextPayloadCache
    {
        bool HasPayloadType(Type payloadType);

        bool TryGetPayload<T>(out T payload)
            where T : class;
        T GetOrAddPayload<T>(PayloadFactory<T> payloadFactory)
            where T : class;

        T AddOrUpdatePayload<T>(PayloadFactory<T> addFactory, UpdatePayloadFactory<T> updateFactory)
            where T : class;

    }

}
