namespace Solari.Rhea
{
    public delegate TOutput RailContextConverterFactory<in TInput, out TOutput>(TInput input);

}
