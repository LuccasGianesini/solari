using System;
using System.Collections.Generic;
using System.Linq;

namespace Solari.Ganymede.Framework
{
    public static class QueryStringHelper
    {
        public static bool Eval(string queryString)
        {
            if (string.IsNullOrEmpty(queryString)) throw new ArgumentException("Value cannot be null or empty.", nameof(queryString));

            if (!queryString.StartsWith('?')) return false;
            if (queryString.EndsWith('&')) return false;
            if (queryString.StartsWith('?') && queryString[1] == '&') return false;

            IDictionary<string, string> dicQueryString = queryString
                                                         .Split('&')
                                                         .ToDictionary(c => c.Split('=')[0],
                                                                       c => Uri.UnescapeDataString(c.Split('=')[1]));
            int ampersandCount = queryString.Count(a => a == '&');

            if (dicQueryString.Count > 1) return dicQueryString.Keys.Count - 1 == ampersandCount;

            return dicQueryString.Keys.Count == ampersandCount;
        }
    }
}