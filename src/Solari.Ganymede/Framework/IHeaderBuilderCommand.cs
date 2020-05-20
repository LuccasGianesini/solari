namespace Solari.Ganymede.Framework
{
    public interface IHeaderBuilderCommand
    {
        void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value);
    }
}