using Solari.Ganymede.Domain.Options;

namespace Solari.Ganymede
{
    public interface IGanymedeRequest<TClientImplementation>
    {
        GanymedeRequestSpecification RequestSpecification { get; }
        GanymedeRequestResource GetResource(string resourceName);
    }
}