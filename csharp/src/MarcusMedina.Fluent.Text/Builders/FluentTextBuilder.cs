namespace MarcusMedina.Fluent.Text.Builders;
using MarcusMedina.Fluent.Text.Extensions.Casing;
/// <summary>
/// Fluent builder for Text operations.
/// </summary>
public class FluentTextBuilder
{
    #region Private Fields

    private readonly string _value;

    #endregion Private Fields

    #region Private Constructors

    private FluentTextBuilder(string value) => _value = value ?? throw new ArgumentNullException(nameof(value));

    #endregion Private Constructors

    #region Public Methods

    /// <summary>
    /// Creates a new instance of the builder.
    /// </summary>
    /// <param name="value">Initial value.</param>
    /// <returns>A new builder instance.</returns>
    /// <example>
    /// <code>
    /// var builder = FluentTextBuilder.From("example");
    /// </code>
    /// </example>
    public static FluentTextBuilder From(string value) => new(value);

    /// <summary>
    /// Builds the final result.
    /// </summary>
    /// <returns>The processed value.</returns>
    public string Build() => _value;

    #endregion Public Methods
}