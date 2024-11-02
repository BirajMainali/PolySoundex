namespace PolySoundex.Services
{
    /// <summary>
    /// Interface for generating Soundex phonetic encoding codes with support for multiple languages and configurations.
    /// The Soundex algorithm encodes similar sounding words to the same representation, allowing for phonetic matching
    /// across different languages and writing systems.
    /// </summary>
    public interface IPolySoundexService
    {
        /// <summary>
        /// Generates a Soundex code for the given input string based on configured language rules.
        /// The Soundex code consists of a letter followed by three numbers, where similar sounding consonants
        /// share the same numeric code.
        /// </summary>
        /// <param name="input">The input string to generate a Soundex code for.</param>
        /// <returns>
        /// A four character Soundex code where:
        /// - First character is the first letter of the input word
        /// - Following three characters are digits based on the phonetic encoding rules
        /// - Returns "0000" for null or empty input
        /// </returns>
        /// <exception cref="PolySoundex.Exceptions.PolySoundexException">
        /// Thrown when no applicable language configuration is found for the input
        /// or when the configuration is invalid.
        /// </exception>
        string GenerateSoundex(string input);
    }
}