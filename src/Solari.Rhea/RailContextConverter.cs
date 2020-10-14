using Solari.Sol;

namespace Solari.Rhea
{
    public class RailContextConverter : IRailContextConverter
    {
        public bool TryConvert<TInput, TOutput>(RailContextConverterFactory<TInput, TOutput> factory, TInput input, out TOutput output)
        {
            Check.ThrowIfNull(factory, nameof(RailContextConverterFactory<TInput, TOutput>));
            Check.ThrowIfNull(input, nameof(TInput));
            output = factory(input);
            if (output is null)
                return false;
            return true;
        }
    }
}
