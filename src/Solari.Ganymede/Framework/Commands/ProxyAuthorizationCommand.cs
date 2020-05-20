namespace Solari.Ganymede.Framework.Commands
{
    internal sealed class ProxyAuthorizationCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value) { headerBuilder.ProxyAuthorization(keyOrQuality, value); }
    }
}