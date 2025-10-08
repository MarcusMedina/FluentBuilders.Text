namespace MarcusMedina.Fluent.Text.Tests.Extensions;

using FluentAssertions;
using MarcusMedina.Fluent.Text.Extensions.Validation;

public class StringValidationExtensionsTests
{
    #region IsEmpty Tests

    [Fact]
    public void IsEmpty_EmptyString_ReturnsTrue()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.IsEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsEmpty_NonEmptyString_ReturnsFalse()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.IsEmpty();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsEmpty_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.IsEmpty();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsEmpty_WhitespaceString_ReturnsFalse()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.IsEmpty();

        // Assert
        result.Should().BeFalse();
    }

    #endregion IsEmpty Tests

    #region IsNullOrEmpty Tests

    [Fact]
    public void IsNullOrEmpty_EmptyString_ReturnsTrue()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_NonEmptyString_ReturnsFalse()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.IsNullOrEmpty();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullOrEmpty_NullString_ReturnsTrue()
    {
        // Arrange
        string? value = null;

        // Act
        var result = value.IsNullOrEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_WhitespaceString_ReturnsFalse()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.IsNullOrEmpty();

        // Assert
        result.Should().BeFalse();
    }

    #endregion IsNullOrEmpty Tests

    #region IsNullOrWhiteSpace Tests

    [Fact]
    public void IsNullOrWhiteSpace_EmptyString_ReturnsTrue()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.IsNullOrWhiteSpace();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrWhiteSpace_NonEmptyString_ReturnsFalse()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.IsNullOrWhiteSpace();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullOrWhiteSpace_NullString_ReturnsTrue()
    {
        // Arrange
        string? value = null;

        // Act
        var result = value.IsNullOrWhiteSpace();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrWhiteSpace_StringWithLeadingWhitespace_ReturnsFalse()
    {
        // Arrange
        var value = "   hello";

        // Act
        var result = value.IsNullOrWhiteSpace();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNullOrWhiteSpace_TabAndNewlineString_ReturnsTrue()
    {
        // Arrange
        var value = "\t\n\r";

        // Act
        var result = value.IsNullOrWhiteSpace();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrWhiteSpace_WhitespaceString_ReturnsTrue()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.IsNullOrWhiteSpace();

        // Assert
        result.Should().BeTrue();
    }

    #endregion IsNullOrWhiteSpace Tests

    #region IsWhiteSpace Tests

    [Fact]
    public void IsWhiteSpace_EmptyString_ReturnsFalse()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.IsWhiteSpace();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsWhiteSpace_MixedWhitespace_ReturnsTrue()
    {
        // Arrange
        var value = " \t\n\r ";

        // Act
        var result = value.IsWhiteSpace();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsWhiteSpace_NonWhitespaceString_ReturnsFalse()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.IsWhiteSpace();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsWhiteSpace_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.IsWhiteSpace();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsWhiteSpace_StringWithText_ReturnsFalse()
    {
        // Arrange
        var value = "   hello   ";

        // Act
        var result = value.IsWhiteSpace();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsWhiteSpace_TabString_ReturnsTrue()
    {
        // Arrange
        var value = "\t\t";

        // Act
        var result = value.IsWhiteSpace();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsWhiteSpace_WhitespaceString_ReturnsTrue()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.IsWhiteSpace();

        // Assert
        result.Should().BeTrue();
    }

    #endregion IsWhiteSpace Tests
}