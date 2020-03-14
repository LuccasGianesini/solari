using System;

namespace Solari.Sol
{
    public class BuildAction
    {
        public BuildAction(string name) { Name = name; }
        public string Name { get;}

        public Action<IServiceProvider> Action { get; set; }
    }
}