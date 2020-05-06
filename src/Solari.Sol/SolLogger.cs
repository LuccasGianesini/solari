using Serilog;

namespace Solari.Sol
{
    public static class SolLogger
    {
        public static class MarshalLogger
        {
            private const string Prefix = "Solari.Sol (Marshal): ";

            public static void ExecutingBuildAction(string actionName) { Log.Debug($"{Prefix}Executing build action: {actionName}"); }
        }
    }
}