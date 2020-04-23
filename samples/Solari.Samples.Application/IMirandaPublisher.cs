using System.Threading.Tasks;
using Solari.Samples.Domain;

namespace Solari.Samples.Application
{
    public interface IMirandaPublisher
    {
        Task PublishTestMessage(TestMessage message);
    }
}