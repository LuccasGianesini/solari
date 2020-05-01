using System;
using Solari.Sol.Extensions;

namespace Solari.Ganymede.Framework.Commands
{
    internal sealed class AcceptCharsetCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value)
        {
            if (headerBuilder == null) throw new ArgumentNullException(nameof(headerBuilder));
            if (value == null) throw new ArgumentNullException(nameof(value));

            headerBuilder.AcceptCharset(value, keyOrQuality.ToDouble());
        }
    }
}