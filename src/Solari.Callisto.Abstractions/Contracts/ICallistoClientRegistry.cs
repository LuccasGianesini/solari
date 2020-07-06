namespace Solari.Callisto.Abstractions.Contracts
{
    public interface ICallistoClientRegistry
    {
        void AddClient(string clientName, ICallistoClient client);
        ICallistoClient GetClient(string clientName);
        void RemoveClient(string clientName);
        void UpdateClient(string clientName, ICallistoClient client);
    }
}
