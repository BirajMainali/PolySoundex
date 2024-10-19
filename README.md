# PolySoundex Library - Developer Documentation

## Overview

PolySoundex is a multi-language Soundex implementation that allows developers to configure Soundex mappings for multiple languages and use them for phonetic search and indexing. The library supports various alphabets, including English and Nepali (Devanagari script), and can be extended to support additional languages.

This documentation provides a guide for configuring and using PolySoundex in your .NET applications, specifically with ASP.NET Core dependency injection.

## Installation

1. **Add the PolySoundex Library** to your .NET project (assume it's available as a NuGet package).

2. **Configure Dependency Injection in your application** using the `AddPolySoundex` method, where you define language-specific Soundex rules and regex patterns for identifying different languages.

## Configuration Example

Here is an example of how you can configure PolySoundex for both English and Nepali languages using the `AddPolySoundex` extension method.

### Dependency Injection Configuration

In your `Program.cs` (for ASP.NET Core applications):

```csharp
builder.Services.AddPolySoundex(config =>
{
    // English Soundex Configuration
    config.Add(new PolySoundexConfig()
    {
        Requirements = new Dictionary<int, char[]>()
        {
            { 1, new[] { 'B', 'F', 'P', 'V' } },
            { 2, new[] { 'C', 'G', 'J', 'K', 'Q', 'S', 'X', 'Z' } },
            { 3, new[] { 'D', 'T' } },
            { 4, new[] { 'L' } },
            { 5, new[] { 'M', 'N' } },
            { 6, new[] { 'R' } }
        },
        LanguageIdentifierRegex = new Regex("^[A-Za-z\\s]+$") // Regex for English alphabet
    });

    // Nepali Soundex (Devanagari script) Configuration
    config.Add(new PolySoundexConfig
    {
        Requirements = new Dictionary<int, char[]>()
        {
            { 1, new[] { 'क', 'ख', 'ग', 'घ' } },
            { 2, new[] { 'च', 'छ', 'ज', 'झ', 'ट', 'ठ', 'ड', 'ढ' } },
            { 3, new[] { 'त', 'थ', 'द', 'ध' } },
            { 4, new[] { 'न' } },
            { 5, new[] { 'प', 'फ', 'ब', 'भ', 'म' } },
            { 6, new[] { 'य', 'र', 'ल', 'व' } }
        },
        LanguageIdentifierRegex = new Regex("^[\u0900-\u097F\\s]+$") // Regex for Devanagari (Nepali) script
    });
});
```

### Explanation:
- **Requirements**: A dictionary mapping integers (Soundex codes) to arrays of characters that share the same phonetic value. For example, in English, 'B', 'F', 'P', and 'V' are treated similarly by Soundex.
- **LanguageIdentifierRegex**: A regular expression used to identify the input language. In this case:
    - `^[A-Za-z]+$` matches English characters.
    - `^[\u0900-\u097F]+$` matches Nepali (Devanagari script) characters.

### Example 

You can now use the `PolySoundex` service in your app to generate Soundex values. Below is an example of how to create an API endpoint that takes user input and returns the Soundex value:

```csharp
app.MapGet("/", (string input, IPolySoundex soundex) =>
{
    try
    {
        var items = new List<(string name, string address)>()
        {
            new("Biraj", "123 Main St."),
            new("Ram", "456 Elm St."),
            new("Hari", "789 Oak St.")
        };

        
        var inputSoundex = soundex.GenerateSoundex(input); // assume input is "Braj" the soundex code is "B620"

        var matchingItems = items
            .Select(i => new { i.name, i.address, Soundex = soundex.GenerateSoundex(i.name.ToUpper()) })
            .Where(i => i.Soundex == inputSoundex)
            .ToList(); // returns { name = "Biraj", address = "123 Main St.", Soundex = "B620" }

        
        return Results.Ok(matchingItems);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

```

### What This Does:
- Accepts a `GET` request with a `string` input.
- Uses the `PolySoundex` service (injected via `IPolySoundex`) to generate the Soundex code for the given input.
- Returns the Soundex result or an error if the language is not supported.

### Example Usage:

1. **English Example**:
    - Input: `Biraj`
    - Soundex Result: `B620`

2. **Nepali Example**:
    - Input: `बिराज` (Biraj in Devanagari)
    - Soundex Result: `ब620`

## How to Extend PolySoundex for Other Languages

To add support for other languages:
1. Define the **Soundex rules** for the language in the `Requirements` dictionary.
2. Create an appropriate **regular expression** for identifying the alphabet or script of the language.
3. Add the new `PolySoundexConfig` in the `AddPolySoundex` configuration.

Example for another language:

```csharp
config.Add(new PolySoundexConfig()
{
    Requirements = new Dictionary<int, char[]>()
    {
        { 1, new[] { 'А', 'О', 'У', 'Э', 'Ю', 'Я' } }, // Example Cyrillic characters
        // Add other mappings for the Cyrillic alphabet
    },
    LanguageIdentifierRegex = new Regex("^[\u0400-\u04FF]+$") // Cyrillic script
});
```

## Error Handling

- **Unsupported Language**: If the input does not match any configured languages, the `PolySoundexException` will be thrown with the message `Language not supported.`
- **Invalid Input**: If the input string does not contain valid characters after being cleaned, an `ArgumentException` will be thrown with a message like `Input string must contain at least one valid character after cleaning.`

## Edge Cases

- **Empty Input**: The Soundex function will return the input as-is.
- **Digits Only Input**: Soundex will return the input unchanged (useful for phone numbers).
- **Input with Mixed Languages**: The `LanguageIdentifierRegex` ensures that input is matched against the correct language. If the input doesn’t fully conform to one language, an error will be thrown.

## Conclusion

PolySoundex offers a flexible and extendable approach to handling multi-language phonetic search and Soundex generation. By configuring language-specific rules and utilizing regex for language identification, you can adapt it to various languages like English, Nepali, and more.

This makes it a powerful tool for building search functionality in applications where phonetic matching of names or other text is crucial, particularly in multi-lingual environments like legal, governmental, or educational systems.
