using System;
using System.Collections.Generic;
using System.Linq;
using PolySoundex.Exceptions;
using PolySoundex.Models;

namespace PolySoundex.Services
{
    /// <summary>
    /// Service for generating Soundex codes based on language-specific configurations.
    /// </summary>
    public class PolySoundexService : IPolySoundexService
    {
        private readonly List<PolySoundexConfig> _configs;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolySoundexService"/> class.
        /// </summary>
        /// <param name="configs">List of PolySoundex configurations.</param>
        public PolySoundexService(List<PolySoundexConfig> configs)
        {
            _configs = configs ?? throw new ArgumentNullException(nameof(configs));
        }

        /// <inheritdoc/>
        public string GenerateSoundex(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var languageConfig = _configs.FirstOrDefault(config => config.IsApplicableLanguage(input));

            if (languageConfig.Equals(default(PolySoundexConfig)))
            {
                return input;
            }

            ValidateConfiguration(languageConfig);

            return GetSoundex(input, languageConfig);
        }

        /// <summary>
        /// Validates the language configuration.
        /// </summary>
        /// <param name="config">The PolySoundex configuration to validate.</param>
        private void ValidateConfiguration(PolySoundexConfig config)
        {
            if (string.IsNullOrEmpty(config.Name))
            {
                throw new PolySoundexException("Configuration name cannot be null or empty.");
            }

            if (config.EnforceStrictValidation && (config.EncodingRules == null || config.EncodingRules.Count == 0))
            {
                throw new PolySoundexException($"Encoding rules must be provided for the {config.Name} configuration.");
            }

            if (config.PrepareInput == null)
            {
                throw new PolySoundexException($"PrepareInput function must be provided for the {config.Name} configuration.");
            }
        }

        /// <summary>
        /// Generates the Soundex code for the given input using the specified language configuration.
        /// </summary>
        /// <param name="input">The input string to generate Soundex for.</param>
        /// <param name="languageConfig">The language-specific configuration to use.</param>
        /// <returns>The generated Soundex code.</returns>
        private string GetSoundex(string input, PolySoundexConfig languageConfig)
        {
            var normalizedInput = languageConfig.PrepareInput(input);

            if (string.IsNullOrEmpty(normalizedInput))
            {
                return new string('0', 4);
            }

            var soundexCode = normalizedInput[0].ToString();
            var lastCode = '0';

            for (var i = 1; i < normalizedInput.Length && soundexCode.Length < 4; i++)
            {
                var currentChar = normalizedInput[i];
                var currentCode = GetEncodingForChar(currentChar, languageConfig.EncodingRules);

                if (currentCode != '0' && currentCode != lastCode)
                {
                    soundexCode += currentCode;
                    lastCode = currentCode;
                }
            }

            return soundexCode.PadRight(4, '0').Substring(0, 4);
        }

        /// <summary>
        /// Gets the encoding for a given character based on the encoding rules.
        /// </summary>
        /// <param name="c">The character to encode.</param>
        /// <param name="encodingRules">The encoding rules to use.</param>
        /// <returns>The encoded character, or '0' if no encoding is found.</returns>
        private char GetEncodingForChar(char c, Dictionary<int, char[]> encodingRules)
        {
            foreach (var kvp in encodingRules)
            {
                if (kvp.Value.Contains(c))
                {
                    return kvp.Key.ToString()[0];
                }
            }
            return '0';
        }
    }
}