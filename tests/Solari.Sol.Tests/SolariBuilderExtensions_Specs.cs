using System;
using System.Dynamic;
using System.Net.Http.Headers;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.ExceptionExtensions;
using Solari.Sol.Abstractions;
using Solari.Sol.Tests.TestSetup;
using Xunit;

namespace Solari.Sol.Tests
{
    public class SolariBuilderExtensions_Specs
    {
        public (IServiceCollection services, IConfiguration configuration) Init(ConfigurationKeys keys, Action<IServiceCollection> registerServices = null)
        {
            IConfiguration config = ConfigurationSetup.BuildConfiguration(keys);
            IServiceCollection services = ContainerSetup.ConfigureContainer(registerServices);
            return (services, config);
        }

        [Fact]
        public void Should_Raise_SolException_When_Application_IConfigurationSection_Does_Not_Exists()
        {
            (IServiceCollection services, IConfiguration configuration) = Init(ConfigurationKeys.NoApplicationSection);
            FluentActions.Invoking(() => services.AddSol(configuration)).Should().Throw<SolariException>().WithMessage("'Application' AppSettings section not found");
        }

        [Fact]
        public void Should_Raise_SolException_When_ApplicationName_Is_Null_Or_Empty()
        {
            (IServiceCollection services, IConfiguration configuration) = Init(ConfigurationKeys.NoApplicationName);
            FluentActions.Invoking(() => services.AddSol(configuration)).Should().Throw<SolariException>().WithMessage("ApplicationName cannot be null or empty");
        }

        [Fact]
        public void Should_Raise_SolException_When_Project_Is_Null_Or_Empty()
        {
            (IServiceCollection services, IConfiguration configuration) = Init(ConfigurationKeys.NoProjectName);
            FluentActions.Invoking(() => services.AddSol(configuration)).Should().Throw<SolariException>().WithMessage("Project cannot be null or empty");
        }
    }
}
