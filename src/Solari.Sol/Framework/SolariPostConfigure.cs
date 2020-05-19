namespace Solari.Sol.Framework
{
    internal sealed class SolariPostConfigure : ISolariPostConfigure
    {
        public SolariPostConfigure(ISolariMarshal marshal) { Marshal = marshal; }

        public ISolariMarshal Marshal { get; }
    }
}