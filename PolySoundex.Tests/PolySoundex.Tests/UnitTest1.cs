using System.Collections.Generic;
using System.Text.RegularExpressions;
using PolySoundex.Models;
using Xunit;

namespace PolySoundex.Tests
{
    public class PolySoundexTests
    {
        private readonly Services.PolySoundex _polySoundex;

        public PolySoundexTests()
        {
            // Configure the PolySoundex instance with English and Nepali configurations
            var configs = new List<PolySoundexConfig>
            {
                new PolySoundexConfig
                {
                    Requirements = new Dictionary<int, char[]>
                    {
                        { 1, new[] { 'B', 'F', 'P', 'V' } },
                        { 2, new[] { 'C', 'G', 'J', 'K', 'Q', 'S', 'X', 'Z' } },
                        { 3, new[] { 'D', 'T' } },
                        { 4, new[] { 'L' } },
                        { 5, new[] { 'M', 'N' } },
                        { 6, new[] { 'R' } }
                    },
                    LanguageIdentifierRegex = new Regex("^[A-Za-z]+$")
                },
                new PolySoundexConfig
                {
                    Requirements = new Dictionary<int, char[]>
                    {
                        { 1, new[] { 'क', 'ख', 'ग', 'घ' } },
                        { 2, new[] { 'च', 'छ', 'ज', 'झ', 'ट', 'ठ', 'ड', 'ढ' } },
                        { 3, new[] { 'त', 'थ', 'द', 'ध' } },
                        { 4, new[] { 'न' } },
                        { 5, new[] { 'प', 'फ', 'ब', 'भ', 'म' } },
                        { 6, new[] { 'य', 'र', 'ल', 'व' } }
                    },
                    LanguageIdentifierRegex = new Regex("^[\u0900-\u097F]+$")
                }
            };

            _polySoundex = new Services.PolySoundex(configs);
        }

        [Theory]
        [InlineData("Biraj", "B620")]
        [InlineData("Braaj", "B620")]
        [InlineData("Braj", "B620")]
        [InlineData("Ram", "R500")]
        public void GenerateSoundex_EnglishNames_ReturnsExpectedSoundex(string input, string expectedSoundex)
        {
            // Act
            var result = _polySoundex.GenerateSoundex(input);

            // Assert
            Assert.Equal(expectedSoundex, result);
        }

        [Theory]
        [InlineData("बिराज", "ब620")]
        [InlineData("बराज", "ब620")]
        public void GenerateSoundex_NepaliNames_ReturnsExpectedSoundex(string input, string expectedSoundex)
        {
            // Act
            var result = _polySoundex.GenerateSoundex(input);

            // Assert
            Assert.Equal(expectedSoundex, result);
        }

        [Fact]
        public void GenerateSoundex_EmptyInput_ReturnsEmptyString()
        {
            // Act
            var result = _polySoundex.GenerateSoundex("");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GenerateSoundex_NumbersOnly_ReturnsInput()
        {
            // Act
            var result = _polySoundex.GenerateSoundex("12345");

            // Assert
            Assert.Equal("12345", result);
        }
    }
}