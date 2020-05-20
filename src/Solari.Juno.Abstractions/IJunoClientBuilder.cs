using VaultSharp.V1.AuthMethods;

namespace Solari.Juno.Abstractions
{
    public interface IJunoClientBuilder
    {
        IJunoClientBuilder WithAddress(string address);
        IJunoClientBuilder WithAuthMethod(IAuthMethodInfo authMethodInfo);
        IJunoClientBuilder WithSecretsPath(string path);
        IJunoClient Build();
    }
}