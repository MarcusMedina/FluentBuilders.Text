namespace MarcusMedina.Fluent.Text.Extensions.LineEndings;

/// <summary>
/// Extension methods for line ending normalization.
/// </summary>
public static class StringLineEndingExtensions
{
    #region Public Methods

    /// <summary>
    /// Normalizes all line endings to the system default (Environment.NewLine).
    /// </summary>
    /// <param name="value">The string to normalize.</param>
    /// <returns>The string with normalized line endings.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello\r\nworld\ntest".NormalizeLineEndings()  // Uses Environment.NewLine
    /// </code>
    /// </example>
    public static string NormalizeLineEndings(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        // First normalize to \n, then convert to system default
        var normalized = value.Replace("\r\n", "\n").Replace("\r", "\n");
        return Environment.NewLine == "\n" ? normalized : normalized.Replace("\n", Environment.NewLine);
    }

    /// <summary>
    /// Converts all line endings to legacy Mac format (\r).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string with legacy Mac line endings.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello\nworld".ToMacLineEndings()  // "hello\rworld"
    /// </code>
    /// </example>
    public static string ToMacLineEndings(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Replace("\r\n", "\r").Replace("\n", "\r");
    }

    /// <summary>
    /// Converts all line endings to Unix format (\n).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string with Unix line endings.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello\r\nworld".ToUnixLineEndings()  // "hello\nworld"
    /// </code>
    /// </example>
    public static string ToUnixLineEndings(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Replace("\r\n", "\n").Replace("\r", "\n");
    }

    /// <summary>
    /// Converts all line endings to Windows format (\r\n).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The string with Windows line endings.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello\nworld".ToWindowsLineEndings()  // "hello\r\nworld"
    /// </code>
    /// </example>
    public static string ToWindowsLineEndings(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        // First normalize to \n, then convert to \r\n
        return value.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
    }

    #endregion Public Methods
}