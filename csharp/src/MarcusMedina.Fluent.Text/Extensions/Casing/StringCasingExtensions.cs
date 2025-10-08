namespace MarcusMedina.Fluent.Text.Extensions.Casing;
#pragma warning disable IDE0058 // Expression value is never used

using System.Globalization;
using System.Text;

/// <summary>
/// Extension methods for string casing transformations.
/// </summary>
public static class StringCasingExtensions
{
    #region Public Methods

    public static string ToAlternatingCase(this string value, bool startWithUpper = true)
    {
        ArgumentNullException.ThrowIfNull(value);

        var result = new StringBuilder(value.Length);
        bool makeUpper = startWithUpper;

        foreach (var c in value)
        {
            if (char.IsLetter(c))
            {
                if (makeUpper)
                {
                    result.Append(char.ToUpper(c, CultureInfo.InvariantCulture));
                }
                else
                {
                    result.Append(char.ToLower(c, CultureInfo.InvariantCulture));
                }

                makeUpper = !makeUpper;
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public static string ToAlternatingCaseInvariant(this string value, bool startWithUpper = true) => ToAlternatingCase(value, startWithUpper);

    /// <summary>
    /// Converts the string to camelCase.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The camelCase string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToCamelCase()  // "helloWorld"
    /// </code>
    /// </example>
    public static string ToCamelCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var pascalCase = ToPascalCase(value);

        return pascalCase.Length == 0 ? pascalCase : char.ToLower(pascalCase[0], CultureInfo.InvariantCulture) + pascalCase[1..];
    }

    /// <summary>
    /// Converts the string to kebab-case.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The kebab-case string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToKebabCase()  // "hello-world"
    /// </code>
    /// </example>
    public static string ToKebabCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
        {
            return value;
        }

        var words = SplitIntoWords(value);
        return string.Join("-", words.Select(w => w.ToLower(CultureInfo.InvariantCulture)));
    }

    public static string ToLeetSpeak(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var leetDict = new Dictionary<char, string>
        {
            ['A'] = "4",
            ['a'] = "4",
            ['E'] = "3",
            ['e'] = "3",
            ['I'] = "1",
            ['i'] = "1",
            ['O'] = "0",
            ['o'] = "0",
            ['S'] = "5",
            ['s'] = "5",
            ['T'] = "7",
            ['t'] = "7",
            ['B'] = "8",
            ['b'] = "8",
            ['G'] = "6",
            ['g'] = "6"
        };

        var result = new StringBuilder(value.Length);

        foreach (var c in value)
        {
            if (leetDict.TryGetValue(c, out var leetChar))
            {
                result.Append(leetChar);
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Converts the string to lowercase.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The lowercase string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "HELLO".ToLowerCase()  // "hello"
    /// </code>
    /// </example>
    public static string ToLowerCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.ToLower(CultureInfo.InvariantCulture);
    }

    public static string ToLowerCaseInvariant(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.ToLower(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the string to name case with smart handling of special patterns.
    /// Handles apostrophes (O'Brien), hyphens (Mary-Jane), Scottish/Irish names (McDonald, MacArthur),
    /// name particles (von Neumann), and Roman numerals (Henry VIII).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The name cased string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "o'brien".ToNameCase()           // "O'Brien"
    /// "mary-jane".ToNameCase()         // "Mary-Jane"
    /// "mcdonald".ToNameCase()          // "McDonald"
    /// "macarthur".ToNameCase()         // "MacArthur"
    /// "von neumann".ToNameCase()       // "von Neumann"
    /// "henry viii".ToNameCase()        // "Henry VIII"
    /// </code>
    /// </example>
    public static string ToNameCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
        {
            return value;
        }

        var words = value.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
        var result = new StringBuilder();

        foreach (var word in words)
        {
            if (result.Length > 0)
            {
                result.Append(' ');
            }

            // Handle name particles (lowercase)
            if (IsNameParticle(word))
            {
                result.Append(word.ToLower(CultureInfo.InvariantCulture));
                continue;
            }

            // Handle Roman numerals (uppercase)
            if (IsRomanNumeral(word))
            {
                result.Append(word.ToUpper(CultureInfo.InvariantCulture));
                continue;
            }

            result.Append(ProcessNameWord(word));
        }

        return result.ToString();
    }

    public static string ToNameCaseInvariant(this string value) => ToNameCase(value);

    /// <summary>
    /// Converts the string to PascalCase.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The PascalCase string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToPascalCase()  // "HelloWorld"
    /// </code>
    /// </example>
    public static string ToPascalCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
        {
            return value;
        }

        var words = SplitIntoWords(value);
        var result = new StringBuilder();

        foreach (var word in words)
        {
            if (word.Length > 0)
            {
                result.Append(char.ToUpper(word[0], CultureInfo.InvariantCulture));
                if (word.Length > 1)
                {
                    result.Append(word[1..].ToLower(CultureInfo.InvariantCulture));
                }
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Alias for ToTitleCase. Converts the string to proper case (each word starts with uppercase).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The proper cased string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToProperCase()  // "Hello World"
    /// </code>
    /// </example>
    public static string ToProperCase(this string value) => ToTitleCase(value);

    public static string ToProperCaseInvariant(this string value) => ToTitleCaseInvariant(value);

    public static string ToRandomCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var random = new Random();
        var result = new StringBuilder(value.Length);

        foreach (var c in value)
        {
            if (char.IsLetter(c))
            {
                if (random.Next(2) == 0)
                {
                    result.Append(char.ToLower(c, CultureInfo.InvariantCulture));
                }
                else
                {
                    result.Append(char.ToUpper(c, CultureInfo.InvariantCulture));
                }
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public static string ToRandomCaseInvariant(this string value) => ToRandomCase(value);

    /// <summary>
    /// Converts the string to SCREAMING_SNAKE_CASE.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The SCREAMING_SNAKE_CASE string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToScreamingSnakeCase()  // "HELLO_WORLD"
    /// </code>
    /// </example>
    public static string ToScreamingSnakeCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
        {
            return value;
        }

        var words = SplitIntoWords(value);
        return string.Join("_", words.Select(w => w.ToUpper(CultureInfo.InvariantCulture)));
    }

    /// <summary>
    /// Converts the string to sentence case (first character uppercase, rest as-is).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The sentence cased string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToSentenceCase()  // "Hello world"
    /// </code>
    /// </example>
    public static string ToSentenceCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Length == 0 ? value : char.ToUpper(value[0], CultureInfo.InvariantCulture) + value[1..];
    }

    public static string ToSentenceCaseInvariant(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Length == 0
            ? value
            : char.ToUpper(value[0], CultureInfo.InvariantCulture) + value[1..].ToLower(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the string to snake_case.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The snake_case string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToSnakeCase()  // "hello_world"
    /// </code>
    /// </example>
    public static string ToSnakeCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0)
        {
            return value;
        }

        var words = SplitIntoWords(value);
        return string.Join("_", words.Select(w => w.ToLower(CultureInfo.InvariantCulture)));
    }

    /// <summary>
    /// Converts the string to title case (each word starts with uppercase).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The title cased string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToTitleCase()  // "Hello World"
    /// </code>
    /// </example>
    public static string ToTitleCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLower(CultureInfo.InvariantCulture));
    }

    public static string ToTitleCaseInvariant(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLower(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Converts the string to uppercase.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The uppercase string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".ToUpperCase()  // "HELLO"
    /// </code>
    /// </example>
    public static string ToUpperCase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.ToUpper(CultureInfo.InvariantCulture);
    }

    public static string ToUpperCaseInvariant(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.ToUpper(CultureInfo.InvariantCulture);
    }

    #endregion Public Methods

    #region Private Methods

    private static string CapitalizeFirst(string word)
    {
        return word.Length == 0
            ? word
            : char.ToUpper(word[0], CultureInfo.InvariantCulture) +
               word[1..].ToLower(CultureInfo.InvariantCulture);
    }

    private static bool IsNameParticle(string word)
    {
        var particles = new[] { "von", "van", "de", "del", "della", "di", "da", "le", "la" };
        return particles.Contains(word.ToLower(CultureInfo.InvariantCulture));
    }

    private static bool IsRomanNumeral(string word)
    {
        var upperWord = word.ToUpper(CultureInfo.InvariantCulture);
        return upperWord.All("IVXLCDM".Contains);
    }

    private static string ProcessNameWord(string word)
    {
        // Handle hyphenated names
        if (word.Contains('-'))
        {
            var parts = word.Split('-');
            return string.Join("-", parts.Select(ProcessNameWord));
        }

        // Handle apostrophes
        if (word.Contains('\''))
        {
            var parts = word.Split('\'');
            if (parts.Length == 2)
            {
                return CapitalizeFirst(parts[0]) + "'" + CapitalizeFirst(parts[1]);
            }
        }

        // Handle Mc/Mac prefixes
        var lower = word.ToLower(CultureInfo.InvariantCulture);
        return lower.StartsWith("mc") && lower.Length > 2
            ? "Mc" + CapitalizeFirst(lower[2..])
            : lower.StartsWith("mac") && lower.Length > 3 ? "Mac" + CapitalizeFirst(lower[3..]) : CapitalizeFirst(word);
    }

    private static string[] SplitIntoWords(string value)
    {
        var words = new List<string>();
        var currentWord = new StringBuilder();

        for (int i = 0; i < value.Length; i++)
        {
            var c = value[i];

            if (char.IsWhiteSpace(c) || c == '-' || c == '_')
            {
                if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
            }
            else if (i > 0 && char.IsUpper(c) && char.IsLower(value[i - 1]))
            {
                // PascalCase boundary
                words.Add(currentWord.ToString());
                currentWord.Clear();
                currentWord.Append(c);
            }
            else
            {
                currentWord.Append(c);
            }
        }

        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
        }

        return [.. words];
    }

    #endregion Private Methods
}