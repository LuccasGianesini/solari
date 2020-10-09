namespace Solari.Rhea
{
    public interface IContextPayloadCache
    {
        bool TryGetPayload<T>(out T payload);

    }
}
