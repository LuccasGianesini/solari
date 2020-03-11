using System;

namespace Solari.Ganymede.Framework.Commands
{
    internal sealed class ExpectCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value)
        {
            if (headerBuilder == null) throw new ArgumentNullException(nameof(headerBuilder));
            if (keyOrQuality == null) throw new ArgumentNullException(nameof(keyOrQuality));

            headerBuilder.Expect(keyOrQuality, value);
        }
    }
}