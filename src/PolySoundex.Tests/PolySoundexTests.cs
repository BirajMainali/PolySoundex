using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PolySoundex.Models;
using PolySoundex.Services;
using Xunit;

namespace PolySoundex.Tests
{
    public class PolySoundexTests
    {
        private readonly PolySoundexService _polySoundexService;

        public PolySoundexTests()
        {
            // Configure the PolySoundex instance with English and Nepali configurations
            var configs = new List<PolySoundexConfig>
            {
                new PolySoundexConfig
                {
                    IsApplicableLanguage = (input) => new Regex("^[A-Za-z]+$").IsMatch(input),
                    PrepareInput = (input) => new Regex("[^A-Za-z]").Replace(input, "").ToUpper(),
                    EnforceStrictValidation = true,
                    Name = "English",
                    EncodingRules = new Dictionary<int, char[]>
                    {
                        { 1, new[] { 'B', 'F', 'P', 'V' } },
                        { 2, new[] { 'C', 'G', 'J', 'K', 'Q', 'S', 'X', 'Z' } },
                        { 3, new[] { 'D', 'T' } },
                        { 4, new[] { 'L' } },
                        { 5, new[] { 'M', 'N' } },
                        { 6, new[] { 'R' } }
                    },
                },
                new PolySoundexConfig
                {
                    IsApplicableLanguage = (input) => new Regex("^[\u0900-\u097F\\s]+$").IsMatch(input),
                    PrepareInput = (input) => new Regex("[^\u0900-\u097F]").Replace(input, ""),
                    EnforceStrictValidation = true,
                    Name = "Nepali",
                    EncodingRules = new Dictionary<int, char[]>()
                    {
                        { 1, new[] { 'क', 'ख', 'ग', 'घ' } },
                        { 2, new[] { 'च', 'छ', 'ज', 'झ', 'ञ', 'ट', 'ठ', 'ड', 'ढ', 'ण', 'श', 'ष', 'स' } },
                        { 3, new[] { 'त', 'थ', 'द', 'ध' } },
                        { 4, new[] { 'न', 'ल' } },
                        { 5, new[] { 'म', 'न' } },
                        { 6, new[] { 'य', 'र', 'व' } }
                    },
                }
            };

            _polySoundexService = new PolySoundexService(configs);
        }

        [Theory]
        [InlineData("Biraj  Mainali", "B620")]
        [InlineData("Braaj", "B620")]
        [InlineData("Braj", "B620")]
        [InlineData("Ram", "R500")]
        [InlineData("Smith", "S530")]
        [InlineData("Smythe", "S530")]
        [InlineData("Johnson", "J525")]
        [InlineData("Jonson", "J525")]
        [InlineData("Williams", "W452")]
        [InlineData("Willyams", "W452")]
        public void GenerateSoundex_EnglishNames_ReturnsExpectedSoundex(string input, string expectedSoundex)
        {
            // Act
            var result = _polySoundexService.GenerateSoundex(input);

            // Assert
            Assert.Equal(expectedSoundex, result);
        }

        [Theory]
        [InlineData("बिराज", "ब620")]
        [InlineData("बराज", "ब620")]
        [InlineData("राम", "र500")]
        [InlineData("कृष्ण", "क200")]
        [InlineData("गोपाल", "ग400")]
        public void GenerateSoundex_NepaliNames_ReturnsExpectedSoundex(string input, string expectedSoundex)
        {
            // Act
            var result = _polySoundexService.GenerateSoundex(input);

            // Assert
            Assert.Equal(expectedSoundex, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GenerateSoundex_EmptyOrNullInput_ReturnsInputAsIs(string input)
        {
            // Act
            var result = _polySoundexService.GenerateSoundex(input);

            // Assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("!@#$%")]
        [InlineData("Mix3d Ch@r5")]
        public void GenerateSoundex_NonAlphabeticInput_ReturnsInputAsIs(string input)
        {
            // Act
            var result = _polySoundexService.GenerateSoundex(input);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void GenerateSoundex_LongInput_ReturnsFourCharacters()
        {
            // Arrange
            var longInput = "Supercalifragilisticexpialidocious";

            // Act
            var result = _polySoundexService.GenerateSoundex(longInput);

            // Assert
            Assert.Equal(4, result.Length);
            Assert.Equal("S162", result);
        }

        [Fact]
        public void GenerateSoundex_MixedLanguageInput_ReturnsInputAsIs()
        {
            // Arrange
            var mixedInput = "EnglishनेपालीMixed";

            // Act
            var result = _polySoundexService.GenerateSoundex(mixedInput);

            // Assert
            Assert.Equal(mixedInput, result);
        }
    }
}