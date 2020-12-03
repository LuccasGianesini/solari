using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;
using Solari.Triton.JetBrains.Annotations;
using Solari.Triton.Utils;

namespace Solari.Triton
{
    public class JsonMapsConverter : JsonConverter
    {
        private readonly List<JsonMapBase> maps;

        public JsonMapsConverter(IEnumerable<JsonMapBase> jsonMaps)
        {
            Check.ThrowIfNull(jsonMaps, nameof(IEnumerable<JsonMapBase>));

            maps = new List<JsonMapBase>(jsonMaps);
            jsonMapsTree = GetJsonMapsTree();
        }

        public ReadOnlyCollection<JsonMapBase> Maps => new ReadOnlyCollection<JsonMapBase>(maps);

        private int blockOnce;

        public override bool CanConvert(Type objectType)
        {
            if (blockOnce == 1)
            {
                blockOnce = 0;
                return false;
            }

            // the question here is: is this objectType associated with any class hierarchy in the `JsonMap` list?
            JsonMapsCache cachedData = GetInfoFromTypeCached(objectType);
            return cachedData != null;
        }

        private readonly Dictionary<Type, JsonMapsCache> dicTypeData = new Dictionary<Type, JsonMapsCache>();

        private readonly JsonMapsTree jsonMapsTree;

        private JsonMapsCache GetInfoFromTypeCached(Type objectType)
        {
            JsonMapsCache data;
            lock (dicTypeData)
                if (!dicTypeData.TryGetValue(objectType, out data))
                {
                    JsonMapBase[] listOfMappers = jsonMapsTree.GetMappers(objectType).ToArray();
                    JsonMapBase[][] subMappers = jsonMapsTree.GetSubpathes(objectType).ToArray();
                    if (listOfMappers.Length == 0 && subMappers.Length == 0)
                        dicTypeData[objectType] = null;
                    else
                        dicTypeData[objectType] = data = new JsonMapsCache { Mappers = listOfMappers, SubMappers = subMappers };
                    return data;
                }
                else
                {
                    return data;
                }
        }

        private JsonMapsTree GetJsonMapsTree()
        {
            var dictionary = new Dictionary<Type, JsonMapsTreeNode>();

            foreach (JsonMapBase jsonMapBase in maps)
            {
                Type current = jsonMapBase.SerializedType;
                JsonMapsTreeNode node;
                if (!dictionary.TryGetValue(current, out node))
                {
                    List<JsonMapBase> acceptedMaps = maps
                                                         .Where(x => x.AcceptsType(current))
                                                         .Where(x => x.SerializedType.GetTypeInfo().IsAssignableFrom(current.GetTypeInfo()))
                                                         .WithMax(x => -TypeDistance(x.SerializedType, current))
                                                         .ToList();

                    if (acceptedMaps.Count == 0)
                        throw new Exception($"`No maps for the type `{current.Name}`");

                    if (acceptedMaps.Count > 1)
                        throw new Exception($"Ambiguous maps for the type `{current.Name}`");

                    Type parent = null;
                    Type? baseTypeInfo = current.GetTypeInfo().BaseType;
                    if (baseTypeInfo != null)
                    {
                        List<JsonMapBase> acceptedParentMaps = maps
                                                                   .Where(x => x.AcceptsType(baseTypeInfo))
                                                                   .Where(x => x.SerializedType.CanBeAssignedFrom(baseTypeInfo))
                                                                   .WithMax(x => -TypeDistance(x.SerializedType, baseTypeInfo))
                                                                   .ToList();

                        if (acceptedParentMaps.Count > 1)
                            throw new Exception($"Ambiguous maps for the type `{current.GetTypeInfo().BaseType?.Name}`");

                        if (acceptedParentMaps.Count > 0 && jsonMapBase is JsonMap)
                            throw new Exception($"Sub mappers must be subclasses of `{nameof(JsonSubclassMap)}`: `{jsonMapBase.GetType().Name}`");

                        if (acceptedParentMaps.Count > 0)
                            parent = acceptedParentMaps[0].SerializedType;
                    }

                    List<JsonMapBase> acceptedChildMaps = maps
                                                              .Where(x => current.CanBeAssignedFrom(x.SerializedType))
                                                              .Where(x => TypeDistance(current, x.SerializedType) == 1)
                                                              .ToList();

                    Type[] children = acceptedChildMaps.Select(x => x.SerializedType).ToArray();

                    if (jsonMapBase is IAndSubtypes && children.Length > 0)
                        throw new Exception($"IAndSubtypes mapper does not accept child mappers.");

                    dictionary[current] = node = new JsonMapsTreeNode(jsonMapBase, parent, children);
                }
            }

            return new JsonMapsTree(dictionary);
        }

        private static int TypeDistance(Type baseType, Type subType)
        {
            var it = 0;
            for (; subType != baseType; it++)
            {
                if (subType == null) throw new Exception("The type does not inherit from the indicated type.");
                subType = subType.GetTypeInfo().BaseType;
            }
            return it;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type objectType = value?.GetType();
            JsonMapsCache cachedData = GetInfoFromTypeCached(objectType);

            while (blockOnce == 1)
                throw new Exception("Writing is blocked but should not.");

            blockOnce = 1;
            JObject jo = JObject.FromObject(value);

            // finding discriminator field names
            string discriminatorFieldValue = null;
            foreach (JsonMapBase jsonMapBase in cachedData.Mappers)
            {
                if (jsonMapBase is IAndSubtypes)
                {
                    if (cachedData.Mappers.Length > 1)
                        throw new Exception("Json mapper of type `IAndSubtypes` cannot be combined with other mappers.");

                    var jsonMapWithSubtypes = jsonMapBase as IAndSubtypes;

                    discriminatorFieldValue = jsonMapWithSubtypes.DiscriminatorFieldValueGetter != null
                        ? jsonMapWithSubtypes.DiscriminatorFieldValueGetter(value.GetType())
                        : value.GetType().AssemblyQualifiedName;

                    jo.Add(jsonMapBase.DiscriminatorFieldName, discriminatorFieldValue);
                    discriminatorFieldValue = null;
                }
                else
                {
                    if (jsonMapBase.DiscriminatorFieldName != null)
                    {
                        if (discriminatorFieldValue == null)
                            throw new Exception("Value of discriminator field not defined by subclass map.");

                        jo.Add(jsonMapBase.DiscriminatorFieldName, discriminatorFieldValue);
                        discriminatorFieldValue = null;
                    }

                    var jsonSubclassMap = jsonMapBase as JsonSubclassMap;
                    if (jsonSubclassMap?.DiscriminatorFieldValue != null)
                        discriminatorFieldValue = jsonSubclassMap.DiscriminatorFieldValue;
                }
            }

            if (discriminatorFieldValue != null)
                throw new Exception("Discriminating value not set to any field.");

            jo.WriteTo(writer, serializer.Converters.ToArray());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // the question here is: is this objectType associated with any class hierarchy in the `JsonMap` list?
            JsonMapsCache cachedData = GetInfoFromTypeCached(objectType);

            while (blockOnce == 1)
                throw new Exception("Writing is blocked but should not.");

            if (reader.TokenType == JsonToken.Null)
            {
                bool isNullable = Nullable.GetUnderlyingType(objectType) != null || !objectType.GetTypeInfo().IsValueType;
                if (isNullable)
                    return null;
            }

            JObject jo = JObject.Load(reader);

            object? value = reader.Value;

            // Looking for discriminators:
            // - match the discriminators of the type given by `objectType`
            // - match the discriminators of subtypes of `objectType`

            // STEP 1: match the discriminators of the type given by `objectType`
            string discriminatorFieldValue = null;
            var foundBaseIAndSubtypes = false;
            JsonMapBase classMapToUse = null;
            foreach (JsonMapBase jsonMapBase in cachedData.Mappers.Reverse())
            {
                if (foundBaseIAndSubtypes)
                    throw new Exception($"IAndSubtypes mapper does not accept child mappers.");

                if (jsonMapBase is IAndSubtypes)
                {
                    if (jsonMapBase.DiscriminatorFieldName != null)
                    {
                        JToken? discriminatorFieldValueToken = jo[jsonMapBase.DiscriminatorFieldName];
                        if (discriminatorFieldValueToken?.Type == JTokenType.String)
                            discriminatorFieldValue = discriminatorFieldValueToken.Value<string>();
                    }

                    foundBaseIAndSubtypes = true;
                    var jsonMapWithSubtypes = jsonMapBase as IAndSubtypes;
                    value = jsonMapWithSubtypes.CreateObject(discriminatorFieldValue);
                    serializer.Populate(jo.CreateReader(), value);
                    discriminatorFieldValue = null;
                }
                else
                {
                    var jsonSubclassMap = jsonMapBase as JsonSubclassMap;
                    if (jsonSubclassMap?.DiscriminatorFieldValue != null && discriminatorFieldValue != null)
                    {
                        if (jsonSubclassMap.DiscriminatorFieldValue != discriminatorFieldValue)
                            throw new Exception($"Discriminator does not match.");

                        discriminatorFieldValue = null;
                    }

                    if (jsonMapBase.DiscriminatorFieldName != null)
                    {
                        JToken? discriminatorFieldValueToken = jo[jsonMapBase.DiscriminatorFieldName];
                        if (discriminatorFieldValueToken?.Type == JTokenType.String)
                            discriminatorFieldValue = discriminatorFieldValueToken.Value<string>();
                    }

                    classMapToUse = jsonMapBase;
                }
            }


            // STEP 2: match the discriminators of subtypes of `objectType`
            JsonMapBase subclassMapToUse = null;
            foreach (JsonMapBase[] jsonSubmappers in cachedData.SubMappers)
            {
                string subDiscriminatorFieldValue = discriminatorFieldValue;
                bool foundSubIAndSubtypes = foundBaseIAndSubtypes;
                JsonMapBase subclassMapToUse2 = null;
                foreach (JsonMapBase submapper in jsonSubmappers.Reverse())
                {
                    if (foundSubIAndSubtypes)
                        throw new Exception($"IAndSubtypes mapper does not accept child mappers.");

                    if (submapper is IAndSubtypes)
                    {
                        if (submapper.DiscriminatorFieldName != null)
                        {
                            JToken? discriminatorFieldValueToken = jo[submapper.DiscriminatorFieldName];
                            if (discriminatorFieldValueToken?.Type == JTokenType.String)
                                subDiscriminatorFieldValue = discriminatorFieldValueToken.Value<string>();

                            if (subDiscriminatorFieldValue == null)
                                throw new Exception($"Discriminator field not found.");
                        }

                        foundSubIAndSubtypes = true;
                        var jsonMapWithSubtypes = submapper as IAndSubtypes;
                        value = jsonMapWithSubtypes.CreateObject(subDiscriminatorFieldValue);
                        serializer.Populate(jo.CreateReader(), value);
                        subDiscriminatorFieldValue = null;
                    }
                    else
                    {
                        var jsonSubclassMap = submapper as JsonSubclassMap;
                        if (jsonSubclassMap != null)
                        {
                            if (jsonSubclassMap.DiscriminatorFieldValue != null)
                            {
                                if (jsonSubclassMap.DiscriminatorFieldValue != subDiscriminatorFieldValue)
                                    break;

                                if (subclassMapToUse != null)
                                    throw new Exception($"Ambiguous maps for the type `{submapper.SerializedType.Name}`");

                                subDiscriminatorFieldValue = null;
                                subclassMapToUse2 = jsonSubclassMap;
                            }
                        }
                        else
                        {
                            if (cachedData.Mappers.Length > 0)
                                throw new Exception($"Sub mappers must be subclasses of `{typeof(JsonSubclassMap).Name}`: `{cachedData.Mappers[0].GetType().Name}`");

                            if (subclassMapToUse != null)
                                throw new Exception($"Ambiguous maps for the type `{submapper.SerializedType.Name}`");

                            subclassMapToUse2 = submapper;
                        }

                        if (submapper.DiscriminatorFieldName != null)
                        {
                            JToken? discriminatorFieldValueToken = jo[submapper.DiscriminatorFieldName];
                            if (discriminatorFieldValueToken?.Type == JTokenType.String)
                                subDiscriminatorFieldValue = discriminatorFieldValueToken.Value<string>();

                            if (subDiscriminatorFieldValue == null)
                                break;
                        }
                    }
                }

                if (subclassMapToUse2 != null)
                    subclassMapToUse = subclassMapToUse2;

                if (subclassMapToUse2 != null && subDiscriminatorFieldValue != null)
                    throw new Exception("Value of discriminator field not verified by any subclass map.");
            }

            if (subclassMapToUse == null && discriminatorFieldValue != null)
                throw new Exception("Value of discriminator field not verified by any subclass map.");

            // STEP 3: Creating the new value with the correct JsonMapBase class if any
            // - by the way, there is no JsonMapBase when a IAndSubtypes mapper is found
            JsonMapBase classMapToUse2 = subclassMapToUse ?? classMapToUse;
            if (classMapToUse2 != null)
            {
                if (!classMapToUse2.CanCreateNew())
                    throw new Exception($"Cannot create object of type `{classMapToUse2.SerializedType}`");

                value = classMapToUse2.CreateNew();
                serializer.Populate(jo.CreateReader(), value);
            }

            return value;
        }
    }
}
