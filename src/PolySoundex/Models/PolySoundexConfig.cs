using System;
using System.Collections.Generic;

namespace PolySoundex.Models
{
    /// <summary>
    /// Configuration class for defining language-specific Soundex encoding rules.
    /// </summary>
    public struct PolySoundexConfig
    {
        /// <summary>
        /// Dictionary mapping numeric codes to arrays of characters that should receive that code.
        /// The key represents the Soundex digit (1-7) and the value array contains all characters
        /// that should be encoded with that digit.
        /// </summary>
        public Dictionary<int, char[]> EncodingRules { get; set; }

        /// <summary>
        /// Function that determines whether an input string belongs to this language configuration.
        /// Returns true if the input string should be processed using this configuration.
        /// </summary>
        public Func<string, bool> IsApplicableLanguage { get; set; }

        /// <summary>
        /// Function to normalize the input string before Soundex encoding.
        /// This can be used to remove unwanted characters or perform any necessary pre-processing.
        /// Returns a Soundex-ready text string.
        /// </summary>
        public Func<string, string> PrepareInput { get; set; }

        /// <summary>
        /// When true, enforces strict validation of the configuration requirements.
        /// </summary>
        public bool EnforceStrictValidation { get; set; }

        /// <summary>
        /// Name of the language configuration.
        /// </summary>
        public string Name { get; set; }
    }
}