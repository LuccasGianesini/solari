namespace Solari.Ganymede.Pipeline
{
    public interface IPipelineStage
    {
        PipelineContext PipelineContext { get; }
    }
}