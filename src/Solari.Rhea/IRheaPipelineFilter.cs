using System.Threading.Tasks;

namespace Solari.Rhea
{
    public interface IRheaPipelineFilter
    {
        Task Call(PipelineContext context);
    }
}
