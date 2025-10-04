namespace MarcusMedina.Fluent.Text.Builders;

/// <summary>
/// Fluent builder for Text operations.
/// </summary>
public class FluentTextBuilder
{
    private readonly string _value;

    private FluentTextBuilder(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

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
    public static FluentTextBuilder From(string value)
    {
        return new FluentTextBuilder(value);
    }

    /// <summary>
    /// Builds the final result.
    /// </summary>
    /// <returns>The processed value.</returns>
    public string Build()
    {
        return _value;
    }
}
