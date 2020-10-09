using System.Threading;

namespace Solari.Rhea
{
    public interface IRailContext
    {
        CancellationToken CancellationToken { get; }
    }
}
