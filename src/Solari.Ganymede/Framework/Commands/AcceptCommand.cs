using System;
using Solari.Sol.Extensions;

namespace Solari.Ganymede.Framework.Commands
{
    public sealed class AcceptCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value)
        {
            if (headerBuilder == null) throw new ArgumentNullException(nameof(headerBuilder));
            if (value == null) throw new ArgumentNullException(nameof(value));

            headerBuilder.Accept(value, keyOrQuality.ToDouble());
        }
    }
}