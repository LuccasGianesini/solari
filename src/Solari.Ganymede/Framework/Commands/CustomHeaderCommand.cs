using System;

namespace Solari.Ganymede.Framework.Commands
{
    public class CustomHeaderCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value)
        {
            if (headerBuilder == null) throw new ArgumentNullException(nameof(headerBuilder));
            if (keyOrQuality == null) throw new ArgumentNullException(nameof(keyOrQuality));
            if (value == null) throw new ArgumentNullException(nameof(value));

            headerBuilder.CustomHeader(keyOrQuality, value);
        }
    }
}