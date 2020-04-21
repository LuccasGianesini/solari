using System.Threading.Tasks;

namespace Solari.Samples.Domain
{
    public interface IGitHubClient
    {
        Task<string> GetUserProfile(string profileName);
    }
}