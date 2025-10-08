namespace MarcusMedina.Fluent.Text.Extensions.Manipulation;

using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Extension methods for string manipulation and transformation.
/// </summary>
public static class StringManipulationExtensions
{
    #region Private Fields

    // Maximum result length: 100MB of chars (200MB memory as char = 2 bytes)
    private const long MaxResultLength = 50_000_000;

    // Compiled regex patterns with timeouts to prevent ReDoS attacks
    private static readonly Regex SentencePattern = new(
        @"(?<=[\.!\?])\s+",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    private static readonly Regex WhitespacePattern = new(
        @"\s+",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Collapses multiple consecutive whitespace characters into a single space.
    /// </summary>
    /// <param name="value">The string to process.</param>
    /// <returns>The string with collapsed whitespace.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello    world  \t  test".CollapseWhitespace()  // "hello world test"
    /// </code>
    /// </example>
    public static string CollapseWhitespace(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return WhitespacePattern.Replace(value, " ");
    }

    /// <summary>
    /// Inserts text at a specified position in the string.
    /// </summary>
    /// <param name="value">The original string.</param>
    /// <param name="index">The position to insert at.</param>
    /// <param name="text">The text to insert.</param>
    /// <returns>The string with inserted text.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="text"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is out of range.</exception>
    /// <example>
    /// <code>
    /// "hello world".InsertAt(6, "beautiful ")  // "hello beautiful world"
    /// </code>
    /// </example>
    public static string InsertAt(this string value, int index, string text)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(text);

        return index < 0 || index > value.Length ? throw new ArgumentOutOfRangeException(nameof(index)) : value.Insert(index, text);
    }

    /// <summary>
    /// Masks part of the string with a specified character.
    /// </summary>
    /// <param name="value">The string to mask.</param>
    /// <param name="start">The starting position to mask.</param>
    /// <param name="length">The number of characters to mask.</param>
    /// <param name="maskChar">The character to use for masking (default: '*').</param>
    /// <returns>The masked string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "1234567890".Mask(4, 4)        // "1234****90"
    /// "password123".Mask(0, 8, '#')  // "########123"
    /// </code>
    /// </example>
    public static string Mask(this string value, int start, int length, char maskChar = '*')
    {
        ArgumentNullException.ThrowIfNull(value);

        if (start < 0 || start >= value.Length || length <= 0)
        {
            return value;
        }

        var endIndex = Math.Min(start + length, value.Length);
        var maskedLength = endIndex - start;

        return value[..start] + new string(maskChar, maskedLength) + value[endIndex..];
    }

    /// <summary>
    /// Removes all whitespace from the string.
    /// </summary>
    /// <param name="value">The string to process.</param>
    /// <returns>The string with all whitespace removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello  world \t test".RemoveWhitespace()  // "helloworldtest"
    /// </code>
    /// </example>
    public static string RemoveWhitespace(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    /// <summary>
    /// Repeats the string a specified number of times.
    /// </summary>
    /// <param name="value">The string to repeat.</param>
    /// <param name="count">The number of times to repeat.</param>
    /// <returns>The repeated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is negative or result would exceed memory limits.</exception>
    /// <example>
    /// <code>
    /// "Ha".Repeat(3)  // "HaHaHa"
    /// </code>
    /// </example>
    public static string Repeat(this string value, int count)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        if (count == 0 || value.Length == 0)
        {
            return string.Empty;
        }

        // Prevent memory exhaustion attacks
        long resultLength = (long)value.Length * count;
        return resultLength > MaxResultLength
            ? throw new ArgumentOutOfRangeException(nameof(count),
                $"Result would be {resultLength} characters, exceeding maximum allowed length of {MaxResultLength}")
            : string.Concat(Enumerable.Repeat(value, count));
    }

    /// <summary>
    /// Reverses the characters in the string.
    /// </summary>
    /// <param name="value">The string to reverse.</param>
    /// <returns>The reversed string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".Reverse()  // "olleh"
    /// </code>
    /// </example>
    public static string Reverse(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var charArray = value.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    /// <summary>
    /// Shuffles the characters in the string randomly.
    /// </summary>
    /// <param name="value">The string to shuffle.</param>
    /// <returns>The string with characters in random order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".Shuffle()  // "lohel" (random)
    /// </code>
    /// </example>
    public static string Shuffle(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var random = new Random();
        var chars = value.ToCharArray();

        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars);
    }

    /// <summary>
    /// Shuffles the sentences in the string randomly.
    /// </summary>
    /// <param name="value">The string containing sentences to shuffle.</param>
    /// <returns>The string with sentences in random order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello. How are you? I'm fine!".ShuffleSentences()
    /// // "I'm fine! Hello. How are you?" (random)
    /// </code>
    /// </example>
    public static string ShuffleSentences(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var sentences = SentencePattern.Split(value)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        var random = new Random();

        for (int i = sentences.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (sentences[i], sentences[j]) = (sentences[j], sentences[i]);
        }

        return string.Join(" ", sentences);
    }

    /// <summary>
    /// Shuffles the words in the string randomly.
    /// </summary>
    /// <param name="value">The string containing words to shuffle.</param>
    /// <returns>The string with words in random order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world today".ShuffleWords()  // "today hello world" (random)
    /// </code>
    /// </example>
    public static string ShuffleWords(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var words = value.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        var random = new Random();

        for (int i = words.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (words[i], words[j]) = (words[j], words[i]);
        }

        return string.Join(" ", words);
    }

    /// <summary>
    /// Truncates the string to a maximum length, optionally adding a suffix.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length including the suffix.</param>
    /// <param name="suffix">The suffix to add when truncating (default: "...").</param>
    /// <returns>The truncated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "This is a long sentence".Truncate(10)       // "This is..."
    /// "This is a long sentence".Truncate(10, "~")  // "This is a~"
    /// </code>
    /// </example>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(suffix);

        if (value.Length <= maxLength)
        {
            return value;
        }

        var truncateAt = maxLength - suffix.Length;
        return truncateAt <= 0 ? suffix[..maxLength] : value[..truncateAt] + suffix;
    }

    /// <summary>
    /// Wraps text at a specified maximum line length.
    /// </summary>
    /// <param name="value">The string to wrap.</param>
    /// <param name="maxLength">The maximum length per line.</param>
    /// <param name="breakWords">If true, breaks words that exceed maxLength; if false, keeps words intact.</param>
    /// <returns>The wrapped string with line breaks.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "This is a very long sentence that needs wrapping".WrapTextAt(20, false)
    /// // "This is a very long\nsentence that needs\nwrapping"
    /// </code>
    /// </example>
    public static string WrapTextAt(this string value, int maxLength, bool breakWords = true)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (maxLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength));
        }

        if (value.Length <= maxLength)
        {
            return value;
        }

        var result = new StringBuilder();
        var currentLine = new StringBuilder();
        var words = value.Split(' ');

        foreach (var word in words)
        {
            if (breakWords && word.Length > maxLength)
            {
                // Break long words
                if (currentLine.Length > 0)
                {
                    result.AppendLine(currentLine.ToString());
                    currentLine.Clear();
                }

                for (int i = 0; i < word.Length; i += maxLength)
                {
                    var chunk = word.Substring(i, Math.Min(maxLength, word.Length - i));
                    result.AppendLine(chunk);
                }
            }
            else
            {
                var testLine = currentLine.Length == 0 ? word : currentLine + " " + word;

                if (testLine.Length <= maxLength)
                {
                    if (currentLine.Length > 0)
                    {
                        currentLine.Append(' ');
                    }

                    currentLine.Append(word);
                }
                else
                {
                    if (currentLine.Length > 0)
                    {
                        result.AppendLine(currentLine.ToString());
                        currentLine.Clear();
                    }

                    currentLine.Append(word);
                }
            }
        }

        if (currentLine.Length > 0)
        {
            result.Append(currentLine);
        }

        return result.ToString().TrimEnd('\r', '\n');
    }

    #endregion Public Methods
}