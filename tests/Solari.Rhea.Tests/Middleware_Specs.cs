using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Solari.Rhea.Tests.TestSetup;
using Solari.Sol;
using Xunit;

namespace Solari.Rhea.Tests
{

    public class Middleware_Specs
    {
        public (IServiceCollection services, IConfiguration configuration) Init(ConfigurationKeys keys, Action<IServiceCollection> registerServices = null)
        {
            IConfiguration config = ConfigurationSetup.BuildConfiguration(keys);
            IServiceCollection services = ContainerSetup.ConfigureContainer(registerServices);
            return (services, config);
        }

        private IServiceCollection RegisterServices()
        {
            var collection = new ServiceCollection();
            // collection.AddSol(RegisterConfiguration())
            //           .AddRhea(a => a.RegisterPipeline<TestPipeline>(b => b.AddFilter<TestFilterOne>()
            //                                                                .AddFilter<TestFilterTwo>()));
            return collection;
        }


        [Fact]
        public async Task Should_Build_Pipeline()
        {
            // IServiceProvider provider = Build();
            // var type = provider.GetRequiredService<TestPipeline>();
            // Assert.NotNull(type);
        }
    }
}
