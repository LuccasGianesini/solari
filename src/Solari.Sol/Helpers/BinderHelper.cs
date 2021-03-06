﻿using Microsoft.Extensions.Configuration;

namespace Solari.Sol.Helpers
{
    internal class BinderHelper
    {
        public static TOptions BindOptions<TOptions>(IConfigurationSection section) where TOptions : class, new()
        {
            var options = new TOptions();
            section.Bind(options);
            return options;
        }
    }
}