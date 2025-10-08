namespace MarcusMedina.Fluent.Text.Tests.Builders;

using FluentAssertions;
using MarcusMedina.Fluent.Text.Builders;

public class FluentTextBuilderTests
{
    #region Public Methods

    [Fact]
    public void Build_ReturnsValue()
    {
        // Arrange
        var builder = FluentTextBuilder.From("test");

        // Act
        var result = builder.Build();

        // Assert
        result.Should().Be("test");
    }

    [Fact]
    public void From_NullValue_ThrowsArgumentNullException()
    {
        // Act
        var act = () => FluentTextBuilder.From(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void From_ValidValue_CreatesBuilder()
    {
        // Act
        var builder = FluentTextBuilder.From("test");

        // Assert
        builder.Should().NotBeNull();
    }

    #endregion Public Methods
}