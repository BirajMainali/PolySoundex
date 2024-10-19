using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PolySoundex.Exceptions;
using PolySoundex.Models;

namespace PolySoundex.Services
{
    public class PolySoundex : IPolySoundex
    {
        private readonly List<PolySoundexConfig> _configs;

        public PolySoundex(List<PolySoundexConfig> configs)
        {
            _configs = configs;
        }

        public string GenerateSoundex(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (input.All(char.IsDigit)) return input;
            var config = _configs.Find(c => c.LanguageIdentifierRegex.IsMatch(input));
            if (config == null) throw new PolySoundexException("No configuration found for the input language.");
            if(config.Requirements == null) throw new PolySoundexException("Requirements must be provided in the configuration.");
            return GetSoundex(input, config.LanguageIdentifierRegex, config.Requirements);
        }

        private string GetSoundex(string input, Regex languageIdentifierRegex, Dictionary<int, char[]> requirement)
        {
            // Normalize the input: convert to uppercase and remove non-alphabetic characters
            input = Regex.Replace(input.ToUpper(), $"[^{languageIdentifierRegex}]", "");
            
            // Initialize Soundex code with the first letter of the filtered input
            var soundexCode = input[0].ToString();

            // Keep track of the last assigned code to avoid consecutive duplicates
            var lastCode = '0';

            // Iterate through the remaining letters to generate the Soundex code
            for (var i = 1; i < input.Length && soundexCode.Length < 4; i++)
            {
                var currentChar = input[i];
                var codeFound = false;

                foreach (var kvp in requirement)
                {
                    if (!kvp.Value.Contains(currentChar)) continue;
                    var currentCode = kvp.Key.ToString()[0]; 
                    if (currentCode != lastCode)
                    {
                        soundexCode += currentCode;
                        lastCode = currentCode;
                    }
                    codeFound = true;
                    break;
                }

                // Reset lastCode if the current character has no corresponding code (e.g., vowels)
                if (!codeFound)
                {
                    lastCode = '0';
                }
            }

            // Pad with zeros or trim to ensure the Soundex code is exactly 4 characters long
            return soundexCode.PadRight(4, '0').Substring(0, 4);
        }
    }
}