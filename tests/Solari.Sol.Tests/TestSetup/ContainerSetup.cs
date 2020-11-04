using Microsoft.Extensions.DependencyInjection;

namespace Solari.Sol.Tests.TestSetup
{
    internal static class ContainerSetup
    {
        public static IServiceCollection EmptyServiceCollection()
        {
            return new ServiceCollection();
        }


    }
}
