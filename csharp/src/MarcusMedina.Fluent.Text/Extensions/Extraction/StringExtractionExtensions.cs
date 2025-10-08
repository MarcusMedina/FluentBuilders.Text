namespace MarcusMedina.Fluent.Text.Extensions.Extraction;

using System.Text.RegularExpressions;

/// <summary>
/// Extension methods for extracting specific data types and patterns from strings.
/// </summary>
public static class StringExtractionExtensions
{
    #region Private Fields

    private static readonly Regex DatePattern = new(
            @"\b\d{1,2}[/-]\d{1,2}[/-]\d{2,4}\b|\b\d{4}[/-]\d{1,2}[/-]\d{1,2}\b",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(100));

    // Compiled regex patterns with timeouts to prevent ReDoS attacks
    private static readonly Regex EmailPattern = new(
        @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    private static readonly Regex HashtagPattern = new(
        @"#\w+",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    private static readonly Regex MentionPattern = new(
        @"@\w+",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    private static readonly Regex NumberPattern = new(
        @"\b\d+(\.\d+)?\b",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    private static readonly Regex PhonePattern = new(
        @"\+?\d[\d -]{8,}\d",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    private static readonly Regex SentencePattern = new(
        @"(?<=[\.!\?])\s+",
        RegexOptions.Compiled | RegexOptions.Multiline,
        TimeSpan.FromMilliseconds(100));

    private static readonly Regex UrlPattern = new(
                            @"(http|https)://[^\s/$.?#].[^\s]*",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Extracts all sentences from the string.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique sentences found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello world. How are you? I'm fine!".ExtractAllSentences()
    /// // ["Hello world.", "How are you?", "I'm fine!"]
    /// </code>
    /// </example>
    public static string[] ExtractAllSentences(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var sentences = SentencePattern.Split(value);
        return sentences.Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all words from the string.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique words found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Hello, world! How are you?".ExtractAllWords()
    /// // ["Hello", "world", "How", "are", "you"]
    /// </code>
    /// </example>
    public static string[] ExtractAllWords(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var words = value.Split(new[] { ' ', '\t', '\n', '\r', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts text between two markers.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="start">The start marker.</param>
    /// <param name="end">The end marker.</param>
    /// <param name="includeMarkers">Whether to include the start and end markers in the result.</param>
    /// <returns>Array of text segments found between the markers.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    /// <example>
    /// <code>
    /// "Hello [world] and [universe]!".ExtractBetween("[", "]")
    /// // ["world", "universe"]
    /// "Hello [world] and [universe]!".ExtractBetween("[", "]", true)
    /// // ["[world]", "[universe]"]
    /// </code>
    /// </example>
    public static string[] ExtractBetween(this string value, string start, string end, bool includeMarkers = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(end);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var pattern = includeMarkers
            ? $@"{Regex.Escape(start)}[^{Regex.Escape(end)}]*{Regex.Escape(end)}"
            : $@"(?<={Regex.Escape(start)})[^{Regex.Escape(end)}]*(?={Regex.Escape(end)})";

        // Use timeout to prevent ReDoS attacks
        var matches = Regex.Matches(value, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
        return matches.Select(m => m.Value).ToArray();
    }

    /// <summary>
    /// Extracts all dates from the string (supports multiple formats).
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique dates found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Meeting on 2024-12-25 and 12/31/2024".ExtractDates()
    /// // ["2024-12-25", "12/31/2024"]
    /// </code>
    /// </example>
    public static string[] ExtractDates(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var matches = DatePattern.Matches(value);
        return matches.Select(m => m.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all email addresses from the string.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique email addresses found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Contact us at info@example.com or sales@test.org".ExtractEmails()
    /// // ["info@example.com", "sales@test.org"]
    /// </code>
    /// </example>
    public static string[] ExtractEmails(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var matches = EmailPattern.Matches(value);
        return matches.Select(m => m.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts hashtags from the string (words starting with #).
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique hashtags found (including the # symbol).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Love #coding and #dotnet!".ExtractHashtags()
    /// // ["#coding", "#dotnet"]
    /// </code>
    /// </example>
    public static string[] ExtractHashtags(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var matches = HashtagPattern.Matches(value);
        return matches.Select(m => m.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts mentions from the string (words starting with @).
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique mentions found (including the @ symbol).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Thanks @john and @jane for the help!".ExtractMentions()
    /// // ["@john", "@jane"]
    /// </code>
    /// </example>
    public static string[] ExtractMentions(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var matches = MentionPattern.Matches(value);
        return matches.Select(m => m.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all numbers from the string (integers and decimals).
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique numbers found as strings.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Price: 19.99 and quantity: 5".ExtractNumbers()
    /// // ["19.99", "5"]
    /// </code>
    /// </example>
    public static string[] ExtractNumbers(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var matches = NumberPattern.Matches(value);
        return matches.Select(m => m.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all phone numbers from the string.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique phone numbers found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Call +46 70 123 4567 or 555-1234".ExtractPhoneNumbers()
    /// // ["+46 70 123 4567", "555-1234"]
    /// </code>
    /// </example>
    public static string[] ExtractPhoneNumbers(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var matches = PhonePattern.Matches(value);
        return matches.Select(m => m.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all URLs from the string.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <returns>Array of unique URLs found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Visit https://example.com or http://test.org".ExtractUrls()
    /// // ["https://example.com", "http://test.org"]
    /// </code>
    /// </example>
    public static string[] ExtractUrls(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        var matches = UrlPattern.Matches(value);
        return matches.Select(m => m.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all words containing the specified substring.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="substring">The substring to search for.</param>
    /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
    /// <returns>Array of unique words containing the substring.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="substring"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world wonderful day".ExtractWordsContaining("or")
    /// // ["world", "wonderful"]
    /// </code>
    /// </example>
    public static string[] ExtractWordsContaining(this string value, string substring, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(substring);

        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(substring))
        {
            return Array.Empty<string>();
        }

        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        var words = value.Split(new[] { ' ', '\t', '\n', '\r', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Where(w => w.Contains(substring, comparison)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all words ending with the specified suffix.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="suffix">The suffix to search for.</param>
    /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
    /// <returns>Array of unique words ending with the suffix.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="suffix"/> is null.</exception>
    /// <example>
    /// <code>
    /// "running jumping walking".ExtractWordsEndingWith("ing")
    /// // ["running", "jumping", "walking"]
    /// </code>
    /// </example>
    public static string[] ExtractWordsEndingWith(this string value, string suffix, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(suffix);

        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(suffix))
        {
            return Array.Empty<string>();
        }

        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        var words = value.Split(new[] { ' ', '\t', '\n', '\r', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Where(w => w.EndsWith(suffix, comparison)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all words of a specific length.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="length">The desired word length.</param>
    /// <returns>Array of unique words of the specified length.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "I am very happy today".ExtractWordsOfLength(4)
    /// // ["very"]
    /// </code>
    /// </example>
    public static string[] ExtractWordsOfLength(this string value, int length)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value) || length <= 0)
        {
            return Array.Empty<string>();
        }

        var words = value.Split(new[] { ' ', '\t', '\n', '\r', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Where(w => w.Length == length).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Extracts all words starting with the specified prefix.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="prefix">The prefix to search for.</param>
    /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
    /// <returns>Array of unique words starting with the prefix.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="prefix"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello happy world".ExtractWordsStartingWith("h")
    /// // ["hello", "happy"]
    /// </code>
    /// </example>
    public static string[] ExtractWordsStartingWith(this string value, string prefix, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(prefix);

        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(prefix))
        {
            return Array.Empty<string>();
        }

        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        var words = value.Split(new[] { ' ', '\t', '\n', '\r', ',', '.', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Where(w => w.StartsWith(prefix, comparison)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Returns the leftmost n characters from the string.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="length">The number of characters to extract.</param>
    /// <returns>The leftmost n characters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="length"/> is negative.</exception>
    /// <example>
    /// <code>
    /// "hello world".Left(5)  // "hello"
    /// "hi".Left(5)           // "hi"
    /// </code>
    /// </example>
    public static string Left(this string value, int length)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        return length >= value.Length ? value : value[..length];
    }

    /// <summary>
    /// Returns a substring from the specified start position with the specified length.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="start">The zero-based starting position.</param>
    /// <param name="length">The number of characters to extract.</param>
    /// <returns>The extracted substring.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="start"/> or <paramref name="length"/> is negative.</exception>
    /// <example>
    /// <code>
    /// "hello world".Mid(6, 5)  // "world"
    /// "hello".Mid(0, 2)        // "he"
    /// </code>
    /// </example>
    public static string Mid(this string value, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(start);
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        return start >= value.Length ? string.Empty : start + length >= value.Length ? value[start..] : value.Substring(start, length);
    }

    /// <summary>
    /// Returns the rightmost n characters from the string.
    /// </summary>
    /// <param name="value">The string to extract from.</param>
    /// <param name="length">The number of characters to extract.</param>
    /// <returns>The rightmost n characters.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="length"/> is negative.</exception>
    /// <example>
    /// <code>
    /// "hello world".Right(5)  // "world"
    /// "hi".Right(5)           // "hi"
    /// </code>
    /// </example>
    public static string Right(this string value, int length)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        return length >= value.Length ? value : value[^length..];
    }

    #endregion Public Methods
}