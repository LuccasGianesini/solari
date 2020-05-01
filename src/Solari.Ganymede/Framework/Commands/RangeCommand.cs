using System;
using Solari.Sol.Extensions;

namespace Solari.Ganymede.Framework.Commands
{
    internal sealed class RangeCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value)
        {
            if (headerBuilder == null) throw new ArgumentNullException(nameof(headerBuilder));
            if (keyOrQuality == null) throw new ArgumentNullException(nameof(keyOrQuality));
            if (value == null) throw new ArgumentNullException(nameof(value));

            headerBuilder.Range(keyOrQuality.ToLong(), value.ToLong());
        }
    }
}