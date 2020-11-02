using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Solari.Sol;
using Xunit;

namespace Solari.Rhea.Tests
{

    public class Middleware_Specs
    {
        private IConfiguration RegisterConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        private IServiceCollection RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddSol(RegisterConfiguration())
                      .AddRhea(a => a.RegisterPipeline<TestPipeline>(b => b.AddFilter<TestFilterOne>()
                                                                           .AddFilter<TestFilterTwo>()));
            return collection;
        }

        private IServiceProvider Build()
        {
            return RegisterServices().BuildServiceProvider();
        }
        [Fact]
        public async Task Should_Build_Pipeline()
        {
            IServiceProvider provider = Build();
            var type = provider.GetRequiredService<TestPipeline>();
            Assert.NotNull(type);
        }
    }
}
