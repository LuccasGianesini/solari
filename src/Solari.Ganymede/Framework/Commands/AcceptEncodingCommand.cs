﻿using System;
using Solari.Io;

namespace Solari.Ganymede.Framework.Commands
{
    public sealed class AcceptEncodingCommand : IHeaderBuilderCommand
    {
        public void Execute(GanymedeHeaderBuilder headerBuilder, string keyOrQuality, string value)
        {
            if (headerBuilder == null) throw new ArgumentNullException(nameof(headerBuilder));
            if (value == null) throw new ArgumentNullException(nameof(value));

            headerBuilder.AcceptEncoding(value, keyOrQuality.ToDouble());
        }
    }
}