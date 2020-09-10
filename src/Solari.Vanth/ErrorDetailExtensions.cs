using Newtonsoft.Json;

namespace Solari.Vanth
{
    public static class ErrorDetailExtensions
    {
        public static string ToString(this ErrorDetail detail) { return JsonConvert.SerializeObject(detail); }
    }
}
