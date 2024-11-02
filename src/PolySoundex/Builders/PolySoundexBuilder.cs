using System;
using System.Collections.Generic;
using PolySoundex.Models;

namespace PolySoundex.Builders
{
    public class PolySoundexBuilder
    {
        private readonly List<PolySoundexConfig> _configs = new();

        public PolySoundexBuilder UsePolySoundex(Action<List<PolySoundexConfig>> configure)
        {
            configure(_configs);
            return this;
        }

        public Services.PolySoundexService Build()
        {
            return new Services.PolySoundexService(_configs);
        }
    }
}