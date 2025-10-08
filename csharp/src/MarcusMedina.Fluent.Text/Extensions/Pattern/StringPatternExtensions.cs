namespace MarcusMedina.Fluent.Text.Extensions.Pattern;

using System.Text.RegularExpressions;

/// <summary>
/// Extension methods for pattern matching and text searching.
/// Inspired by SQL-style operations, useful for teaching pattern matching concepts.
/// </summary>
public static class StringPatternExtensions
{
    #region Public Methods

    /// <summary>
    /// Checks if the string is between two values (alphabetically).
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="start">The start of the range (inclusive).</param>
    /// <param name="end">The end of the range (inclusive).</param>
    /// <param name="caseSensitive">Whether the comparison should be case-sensitive.</param>
    /// <returns>True if the string is between start and end (inclusive); otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    /// <example>
    /// <code>
    /// "bob".Between("alice", "charlie")    // true
    /// "alice".Between("alice", "charlie")  // true (inclusive)
    /// "dave".Between("alice", "charlie")   // false
    /// </code>
    /// </example>
    public static bool Between(this string value, string start, string end, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(end);

        var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        return string.Compare(value, start, comparison) >= 0 && string.Compare(value, end, comparison) <= 0;
    }

    /// <summary>
    /// Checks if the string contains the search term.
    /// </summary>
    /// <param name="value">The string to search in.</param>
    /// <param name="searchTerm">The term to search for.</param>
    /// <param name="caseSensitive">Whether the search should be case-sensitive.</param>
    /// <returns>True if the string contains the search term; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="searchTerm"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ContainsText("world")  // true
    /// "hello world".ContainsText("WORLD")  // true (case-insensitive)
    /// "hello world".ContainsText("xyz")    // false
    /// </code>
    /// </example>
    public static bool ContainsText(this string value, string searchTerm, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(searchTerm);

        return value.Contains(searchTerm, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the string ends with the specified suffix.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="suffix">The suffix to check for.</param>
    /// <param name="caseSensitive">Whether the check should be case-sensitive.</param>
    /// <returns>True if the string ends with the suffix; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="suffix"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".EndsWithText("world")  // true
    /// "hello world".EndsWithText("WORLD")  // true (case-insensitive)
    /// "hello world".EndsWithText("hello")  // false
    /// </code>
    /// </example>
    public static bool EndsWithText(this string value, string suffix, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(suffix);

        return value.EndsWith(suffix, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the string matches any value in the collection.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="values">The collection of values to check against.</param>
    /// <param name="caseSensitive">Whether the comparison should be case-sensitive.</param>
    /// <returns>True if the string matches any value in the collection; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="values"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".In(["hello", "world"])        // true
    /// "HELLO".In(["hello", "world"])        // true (case-insensitive)
    /// "test".In(["hello", "world"])         // false
    /// </code>
    /// </example>
    public static bool In(this string value, IEnumerable<string> values, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(values);

        var comparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        return values.Contains(value, comparer);
    }

    /// <summary>
    /// Performs a SQL LIKE pattern match on the string.
    /// Supports % (zero or more characters) and _ (exactly one character) wildcards.
    /// </summary>
    /// <param name="value">The string to search in.</param>
    /// <param name="pattern">The SQL LIKE pattern (case-insensitive by default).</param>
    /// <param name="caseSensitive">Whether the match should be case-sensitive.</param>
    /// <returns>True if the string matches the pattern; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="pattern"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".IsLike("hello%")      // true
    /// "hello world".IsLike("%world")      // true
    /// "hello world".IsLike("hello_world") // true
    /// "hello world".IsLike("h%d")         // true
    /// "hello world".IsLike("HELLO%")      // true (case-insensitive)
    /// </code>
    /// </example>
    public static bool IsLike(this string value, string pattern, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(pattern);

        // Convert SQL LIKE pattern to regex
        var regexPattern = "^" + Regex.Escape(pattern)
            .Replace("%", ".*")
            .Replace("_", ".") + "$";

        var options = caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;

        // Use timeout to prevent ReDoS attacks
        return Regex.IsMatch(value, regexPattern, options, TimeSpan.FromMilliseconds(100));
    }

    /// <summary>
    /// Checks if the string starts with the specified prefix.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="prefix">The prefix to check for.</param>
    /// <param name="caseSensitive">Whether the check should be case-sensitive.</param>
    /// <returns>True if the string starts with the prefix; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> or <paramref name="prefix"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".StartsWithText("hello")  // true
    /// "hello world".StartsWithText("HELLO")  // true (case-insensitive)
    /// "hello world".StartsWithText("world")  // false
    /// </code>
    /// </example>
    public static bool StartsWithText(this string value, string prefix, bool caseSensitive = false)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(prefix);

        return value.StartsWith(prefix, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
    }

    #endregion Public Methods
}