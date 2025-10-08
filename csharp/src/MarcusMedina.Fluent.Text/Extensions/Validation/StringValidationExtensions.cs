namespace MarcusMedina.Fluent.Text.Extensions.Validation;

/// <summary>
/// Extension methods for string validation.
/// </summary>
public static class StringValidationExtensions
{
    #region Public Methods

    /// <summary>
    /// Determines whether the string is empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is empty; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "".IsEmpty()      // true
    /// "hello".IsEmpty() // false
    /// </code>
    /// </example>
    public static bool IsEmpty(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Length == 0;
    }

    /// <summary>
    /// Determines whether the string is null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is null or empty; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// string? s = null;
    /// s.IsNullOrEmpty()      // true
    /// "".IsNullOrEmpty()     // true
    /// "hello".IsNullOrEmpty() // false
    /// </code>
    /// </example>
    public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

    /// <summary>
    /// Determines whether the string is null, empty, or consists only of whitespace.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is null, empty, or whitespace; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// string? s = null;
    /// s.IsNullOrWhiteSpace()      // true
    /// "".IsNullOrWhiteSpace()     // true
    /// "   ".IsNullOrWhiteSpace()  // true
    /// "hello".IsNullOrWhiteSpace() // false
    /// </code>
    /// </example>
    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Determines whether the string consists only of whitespace.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is non-empty and consists only of whitespace; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "   ".IsWhiteSpace()  // true
    /// "".IsWhiteSpace()     // false (empty, not whitespace)
    /// "hello".IsWhiteSpace() // false
    /// </code>
    /// </example>
    public static bool IsWhiteSpace(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Length > 0 && value.All(char.IsWhiteSpace);
    }

    #endregion Public Methods
}