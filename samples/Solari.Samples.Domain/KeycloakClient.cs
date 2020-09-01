using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Solari.Ganymede;
using Solari.Ganymede.Pipeline;

namespace Solari.Samples.Domain
{
    public interface IKeycloakClient
    {
        Task<IdentityModel> Signin(AuthModel model);
    }

    public class KeycloakClient : GanymedeClient<KeycloakClient>, IKeycloakClient
    {
        private readonly OidcOptions _options;

        public KeycloakClient(HttpClient httpClient, IGanymedeRequest<KeycloakClient> request, IOptions<OidcOptions> options)
            : base(httpClient, request)
        {
            _options = options.Value;
        }


        public async Task<IdentityModel> Signin(AuthModel model)
        {
            model.client_id = _options.ClientId;
            model.client_secret = _options.ClientSecret;

            var result = await NewRequest.ForResource("signin")
                                         .ConfigureRequestContent(a => a.SerializeContent(model, "application/x-www-form-urlencoded"))
                                         .Send()
                                         .As<IdentityModel>();
            return result;
        }
    }
}
