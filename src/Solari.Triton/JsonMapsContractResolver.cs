using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Solari.Triton.JetBrains.Annotations;

namespace Solari.Triton
{
    /// <summary>
    /// Special JsonConvert resolver that allows the usage of <see cref="JsonMap"/> objects.
    /// </summary>
    public class JsonMapsContractResolver : DefaultContractResolver
    {
        // Based on http://stackoverflow.com/a/13588192/1037948

        private readonly List<JsonMapBase> maps;

        public JsonMapsContractResolver([NotNull] IEnumerable<JsonMapBase> jsonMaps)
        {
            if (jsonMaps == null)
                throw new ArgumentNullException(nameof(jsonMaps));

            this.maps = new List<JsonMapBase>(jsonMaps);
        }

        public ReadOnlyCollection<JsonMapBase> Maps => new ReadOnlyCollection<JsonMapBase>(this.maps);

        /// <summary>
        /// Creates and applyes all actions to a <see cref="JsonProperty"/>.
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            foreach (JsonMapBase map in this.maps)
                if (map.AcceptsType(member.DeclaringType))
                    foreach (Action<MemberInfo, JsonProperty, MemberSerialization> action in map.Actions)
                        action?.Invoke(member, property, memberSerialization);

            return property;
        }

        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            return this.maps.Any(x => x.AcceptsType(objectType))
                ? null
                : base.ResolveContractConverter(objectType);
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            return this.maps.Any(x => x.AcceptsType(objectType))
                ? this.CreateObjectContract(objectType)
                : base.CreateContract(objectType);
        }
    }
}
