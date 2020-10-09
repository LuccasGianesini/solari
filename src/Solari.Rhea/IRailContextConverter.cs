namespace Solari.Rhea
{
    public interface IRailContextConverter
    {
        bool TryConvert<TInput, TOutput>(RailContextConverterFactory<TInput, TOutput> factory, TInput input);
    }
}
