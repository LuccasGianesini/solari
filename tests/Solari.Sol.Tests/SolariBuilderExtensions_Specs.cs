using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solari.Sol.Abstractions;
using Solari.Sol.Tests.TestSetup;
using Xunit;

namespace Solari.Sol.Tests
{
    public class SolariBuilderExtensions_Specs
    {
        [Fact]
        public void Should_Raise_SolException_When_Application_IConfigurationSection_Does_Not_Exists()
        {
            // Arrange
            IConfiguration config = ConfigurationSetup.BuildConfiguration(SolTestsConstants.NoAppSectionAppSettings);
            IServiceCollection services = ContainerSetup.EmptyServiceCollection();

            // Act/Assert
            Assert.Throws<SolariException>(() => services.AddSol(config));
        }

        [Fact]
        public void Should_Raise_SolException_When_ApplicationName_Is_Null_Or_Empty()
        {
            // Arrange
            IConfiguration config = ConfigurationSetup.BuildConfiguration(SolTestsConstants.NoAppNameAppSettings);
            IServiceCollection services = ContainerSetup.EmptyServiceCollection();

            // Act/Assert
            Assert.Throws<SolariException>(() => services.AddSol(config));
        }

        [Fact]
        public void Should_Raise_SolException_When_Project_Is_Null_Or_Empty()
        {
            // Arrange
            IConfiguration config = ConfigurationSetup.BuildConfiguration(SolTestsConstants.NoProjectAppSettings);
            IServiceCollection services = ContainerSetup.EmptyServiceCollection();

            // Act/Assert
            Assert.Throws<SolariException>(() => services.AddSol(config));
        }
    }
}
