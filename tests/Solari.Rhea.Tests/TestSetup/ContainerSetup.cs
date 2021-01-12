using System;
using Microsoft.Extensions.DependencyInjection;

namespace Solari.Rhea.Tests.TestSetup
{
    public class ContainerSetup
    {
        public static IServiceCollection ConfigureContainer(Action<IServiceCollection> registerServices = null)
        {
            var collection = new ServiceCollection();
            registerServices?.Invoke(collection);
            return collection;
        }
    }
}
