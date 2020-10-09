using Solari.Sol;

namespace Solari.Rhea
{
    public class RailContextConverter : IRailContextConverter
    {
        public bool TryConvert<TInput, TOutput>(RailContextConverterFactory<TInput, TOutput> factory, TInput input)
        {
            Check.ThrowIfNull(factory, nameof(RailContextConverterFactory<TInput, TOutput>));
            Check.ThrowIfNull(input, nameof(TInput));
        }
    }
}
