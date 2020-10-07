using System;
using FluentJsonNet;
using Newtonsoft.Json;

namespace Solari.Sol
{
    public static class JsonMapHelper
    {
        public static JsonSerializerSettings CreateSettings(Type type)
        {
            return JsonMaps.GetDefaultSettings(new[] {type}).Invoke();
        }
        public static JsonSerializerSettings CreateSettings(Type[] types)
        {
            return JsonMaps.GetDefaultSettings(types).Invoke();
        }

    }
}
