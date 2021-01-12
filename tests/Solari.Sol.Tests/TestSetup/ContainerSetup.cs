using System;
using Microsoft.Extensions.DependencyInjection;

namespace Solari.Sol.Tests.TestSetup
{
    internal static class ContainerSetup
    {
        public static IServiceCollection ConfigureContainer(Action<IServiceCollection> registerServices = null)
        {
            var collection = new ServiceCollection();
            registerServices?.Invoke(collection);
            return collection;
        }
    }
}
