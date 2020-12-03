using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Triton.JetBrains.Annotations;

namespace Solari.Triton
{

    public static class SettingsBuilder
    {
        public static JsonSerializerSettings CreateWithJsonMaps(Type[] types)
        {
            Check.ThrowIfNullOrEmpty(types, nameof(types));
            JsonMapBase[] jsonMaps = GetJsomJsonMaps(types);
            return new JsonSerializerSettings()
            {
                ContractResolver = CreateContractResolver(jsonMaps),
                Converters = CreateConverters(jsonMaps)
            };
        }

        public static JsonSerializerSettings CreateWithJsonMaps(Type[] types, Func<JsonSerializerSettings, JsonSerializerSettings> configure)
        {
            Check.ThrowIfNull(configure, nameof(Func<JsonSerializerSettings, JsonSerializerSettings>));
            return configure(CreateWithJsonMaps(types));
        }


        public static JsonSerializerSettings SetJsonMapResolver(this JsonSerializerSettings settings, Type[] types)
        {
            Check.ThrowIfNullOrEmpty(types, nameof(types));
            JsonMapBase[] maps = GetJsomJsonMaps(types);
            settings.Converters = CreateConverters(maps);
            settings.ContractResolver = CreateContractResolver(maps);
            return settings;
        }

        private static JsonMapBase[] GetJsomJsonMaps(Type[] types)
        {
            return types
                   .Select(t => t.GetTypeInfo())
                   .Where(t => typeof(JsonMapBase).GetTypeInfo().IsAssignableFrom(t))
                   .Where(t => !t.ContainsGenericParameters && !t.IsAbstract && t.GetConstructor(new Type[0]) != null)
                   .Select(t => Activator.CreateInstance(t.AsType()))
                   .Cast<JsonMapBase>()
                   .ToArray();
        }

        private static IList<JsonConverter> CreateConverters(IEnumerable<JsonMapBase> jsonMaps)
        {
            var result = new JsonMapsConverter(jsonMaps);
            return new List<JsonConverter>(jsonMaps.Count()) {result}.AsReadOnly();
        }

        private static IContractResolver CreateContractResolver(IEnumerable<JsonMapBase> jsonMaps)
        {
            return new JsonMapsContractResolver(jsonMaps);
        }
    }
}
