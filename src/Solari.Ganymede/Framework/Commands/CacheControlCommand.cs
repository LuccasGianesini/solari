using System;

namespace Solari.Ganymede.Framework.Commands
{
    internal sealed class CacheControlCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value)
        {
            if (headerBuilder == null) throw new ArgumentNullException(nameof(headerBuilder));
            if (value == null) throw new ArgumentNullException(nameof(value));

            headerBuilder.CacheControl(value);
        }
    }
}