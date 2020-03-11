using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Framework;
using Solari.Sol;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Solari.Ganymede.DependencyInjection
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddGanymede(this ISolariBuilder builder, Action<HttpClientActions> configureClients,
                                                 string yamlFile = GanymedeConstants.YamlFileName,
                                                 Action<PolicyActions> configurePoliceRegistry = null)
        {
            AddCoreServices(builder, yamlFile);
            configurePoliceRegistry?.Invoke(new PolicyActions(builder));
            configureClients(new HttpClientActions(builder, BuildClientSettings(yamlFile, builder)));

            return builder;
        }

        private static ISolariBuilder AddCoreServices(ISolariBuilder builder, string yamlFile)
        {
            builder.Services.Configure<GanymedePolicyOptions>(builder.AppConfiguration.GetSection(GanymedeConstants.HttpPolicies));
            builder
                .Services
                .AddSingleton<GanymedePolicyRegistry>()
                .AddPolicyRegistry();
            builder.Services.TryAddSingleton(provider => provider.GetRequiredService<GanymedePolicyRegistry>());
            builder.Services.AddTransient<HttpRequestCoordinator>();

            return builder;
        }

        private static IReadOnlyDictionary<string, GanymedeRequestSettings> BuildClientSettings(string yamlFileName, ISolariBuilder builder)
        {
            string file = builder.HostEnvironment.IsProduction()
                              ? Path.Join(AppContext.BaseDirectory, yamlFileName + ".yaml")
                              : Path.Join(AppContext.BaseDirectory, yamlFileName + "." + builder.HostEnvironment.EnvironmentName + ".yaml");


            if (File.Exists(file))
            {
                return ReadAndDeserializeYaml(file).ToDictionary(pair => pair.Name, pair => pair);
            }

            throw new Exception($"Could not find {yamlFileName}.yaml");
        }

        private static IEnumerable<GanymedeRequestSettings> ReadAndDeserializeYaml(string yamlPath)
        {
            using (var stream = new StreamReader(yamlPath))
            {
                return JsonConvert.DeserializeObject<List<GanymedeRequestSettings>>(new SerializerBuilder()
                                                                                   .JsonCompatible()
                                                                                   .Build()
                                                                                   .Serialize(new DeserializerBuilder()
                                                                                              .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                                                                              .Build()
                                                                                              .Deserialize(new StringReader(stream.ReadToEnd()))));
            }
        }
    }
}