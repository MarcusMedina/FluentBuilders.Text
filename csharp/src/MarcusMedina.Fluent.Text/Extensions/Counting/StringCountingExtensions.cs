namespace MarcusMedina.Fluent.Text.Extensions.Counting;

/// <summary>
/// Extension methods for counting and statistical operations on strings.
/// </summary>
public static class StringCountingExtensions
{
    #region Public Methods

    /// <summary>
    /// Counts the number of consonants in the string.
    /// </summary>
    /// <param name="value">The string to count consonants in.</param>
    /// <param name="caseSensitive">Whether to treat uppercase and lowercase differently.</param>
    /// <returns>The number of consonants.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello World".CountConsonants()  // 7 (H, l, l, W, r, l, d)
    /// </code>
    /// </example>
    public static int CountConsonants(this string value, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);

        var vowels = new[] { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
        var searchValue = caseSensitive ? value : value.ToLowerInvariant();

        return searchValue.Count(c => char.IsLetter(c) && !vowels.Contains(caseSensitive ? c : char.ToLowerInvariant(c)));
    }

    /// <summary>
    /// Counts the number of digits in the string.
    /// </summary>
    /// <param name="value">The string to count digits in.</param>
    /// <returns>The number of digits.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "abc123def456".CountDigits()  // 6
    /// </code>
    /// </example>
    public static int CountDigits(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Count(char.IsDigit);
    }

    /// <summary>
    /// Counts the number of letters in the string.
    /// </summary>
    /// <param name="value">The string to count letters in.</param>
    /// <returns>The number of letters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello World 123!".CountLetters()  // 10
    /// </code>
    /// </example>
    public static int CountLetters(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Count(char.IsLetter);
    }

    /// <summary>
    /// Counts the number of lines in the string.
    /// </summary>
    /// <param name="value">The string to count lines in.</param>
    /// <returns>The number of lines.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "line1\nline2\nline3".CountLines()  // 3
    /// </code>
    /// </example>
    public static int CountLines(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return string.IsNullOrWhiteSpace(value) ? 0 : value.Split('\n').Length;
    }

    /// <summary>
    /// Counts the number of lowercase letters in the string.
    /// </summary>
    /// <param name="value">The string to count lowercase letters in.</param>
    /// <returns>The number of lowercase letters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello World".CountLowercase()  // 8 (e,l,l,o,o,r,l,d)
    /// </code>
    /// </example>
    public static int CountLowercase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Count(char.IsLower);
    }

    /// <summary>
    /// Counts the number of occurrences of a character in the string.
    /// </summary>
    /// <param name="value">The string to search in.</param>
    /// <param name="character">The character to count.</param>
    /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
    /// <returns>The number of occurrences.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".CountOccurrences('l')  // 3
    /// "Hello World".CountOccurrences('o', false)  // 2
    /// </code>
    /// </example>
    public static int CountOccurrences(this string value, char character, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (caseSensitive)
        {
            return value.Count(c => c == character);
        }
        else
        {
            var lowerChar = char.ToLowerInvariant(character);
            return value.Count(c => char.ToLowerInvariant(c) == lowerChar);
        }
    }

    /// <summary>
    /// Counts the number of occurrences of a substring in the string.
    /// </summary>
    /// <param name="value">The string to search in.</param>
    /// <param name="substring">The substring to count.</param>
    /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
    /// <returns>The number of occurrences.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="substring"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello hello world".CountOccurrences("hello")  // 2
    /// "Hello HELLO hello".CountOccurrences("hello", false)  // 3
    /// </code>
    /// </example>
    public static int CountOccurrences(this string value, string substring, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(substring);

        if (substring.Length == 0)
        {
            return 0;
        }

        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        int count = 0;
        int index = 0;

        while ((index = value.IndexOf(substring, index, comparison)) != -1)
        {
            count++;
            index += substring.Length;
        }

        return count;
    }

    /// <summary>
    /// Counts the number of sentences in the string.
    /// </summary>
    /// <param name="value">The string to count sentences in.</param>
    /// <returns>The number of sentences.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello. How are you? I'm fine!".CountSentences()  // 3
    /// </code>
    /// </example>
    public static int CountSentences(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        var sentencePattern = @"(?<=[\.!\?])\s+";
        var sentences = System.Text.RegularExpressions.Regex.Split(value, sentencePattern)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        return sentences.Length;
    }

    /// <summary>
    /// Counts the number of uppercase letters in the string.
    /// </summary>
    /// <param name="value">The string to count uppercase letters in.</param>
    /// <returns>The number of uppercase letters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello World".CountUppercase()  // 2 (H, W)
    /// </code>
    /// </example>
    public static int CountUppercase(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value.Count(char.IsUpper);
    }

    /// <summary>
    /// Counts the number of vowels in the string.
    /// </summary>
    /// <param name="value">The string to count vowels in.</param>
    /// <param name="caseSensitive">Whether to treat uppercase and lowercase differently.</param>
    /// <returns>The number of vowels.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello World".CountVowels()  // 3 (e, o, o)
    /// </code>
    /// </example>
    public static int CountVowels(this string value, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);

        var vowels = caseSensitive
            ? new[] { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' }
            : new[] { 'a', 'e', 'i', 'o', 'u' };

        var searchValue = caseSensitive ? value : value.ToLowerInvariant();

        return searchValue.Count(c => vowels.Contains(c));
    }

    /// <summary>
    /// Counts the number of words in the string.
    /// </summary>
    /// <param name="value">The string to count words in.</param>
    /// <returns>The number of words.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello world, how are you?".CountWords()  // 5
    /// </code>
    /// </example>
    public static int CountWords(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        var words = value.Split(new[] { ' ', '\t', '\n', '\r', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    #endregion Public Methods
}