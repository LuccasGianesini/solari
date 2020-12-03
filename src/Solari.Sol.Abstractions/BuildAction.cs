using System;
using System.Threading.Tasks;

namespace Solari.Sol.Abstractions
{
    public class BuildAction
    {
        public BuildAction(string name) { Name = name; }
        public string Name { get; }

        public Action<IServiceProvider> Action { get; set; }

        public Task<Action<IServiceProvider>> AsyncAction { get; set; }
    }
}
