using System;

namespace Solari.Deimos.Elastic
{
    public static class EnvVarHelper
    {
        public static void TrySetValue(string envVar, string value)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envVar)))
            {
                Environment.SetEnvironmentVariable(envVar, value);
            }
        }
    }
}