using System.Net.Http;
using System.Threading.Tasks;
using Solari.Ganymede;
using Solari.Ganymede.Pipeline;

namespace Solari.Samples.Domain
{
    public class GitHubClient : GanymedeClient<GitHubClient>, IGitHubClient
    {
        public GitHubClient(HttpClient httpClient, IGanymedeRequest<GitHubClient> request) : base(httpClient, request)
        {
        }

        public async Task<string> GetUserProfile(string profileName)
        {
            string result = await NewRequest.ForResource("Get-User-Profile")
                                            .ConfigureRequestUri(a => a.ReplaceToken("user-name", profileName))
                                            .Send()
                                            .AsString();
            return result;
        }
        
    }
}