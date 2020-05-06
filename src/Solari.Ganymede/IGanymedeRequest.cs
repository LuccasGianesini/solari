using Solari.Ganymede.Domain.Options;

namespace Solari.Ganymede
{
    public interface IGanymedeRequest<TClientImplementation>
    {
        GanymedeRequestSettings RequestSettings { get; }
        GanymedeRequestResource GetResource(string resourceName);
    }
}