using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PolySoundex.Models
{
    public class PolySoundexConfig
    {
        public Dictionary<int, char[]> Requirements { get; set; }
        public Regex LanguageIdentifierRegex { get; set; }
    }
}