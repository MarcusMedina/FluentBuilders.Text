#pragma warning disable IDE0058 // Expression value is never used

namespace MarcusMedina.Fluent.Text.Tests.Extensions;

using FluentAssertions;
using MarcusMedina.Fluent.Text.Extensions.Pattern;

public class StringPatternExtensionsTests
{
    #region Between Tests

    [Fact]
    public void Between_ValueInRange_ReturnsTrue()
    {
        // Arrange
        var value = "bob";

        // Act
        var result = value.Between("alice", "charlie");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Between_ValueAtStart_ReturnsTrue()
    {
        // Arrange
        var value = "alice";

        // Act
        var result = value.Between("alice", "charlie");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Between_ValueOutOfRange_ReturnsFalse()
    {
        // Arrange
        var value = "dave";

        // Act
        var result = value.Between("alice", "charlie");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Between_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Between("a", "z");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion Between Tests

    #region ContainsText Tests

    [Fact]
    public void ContainsText_StringContainsTerm_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ContainsText("world");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsText_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ContainsText("WORLD");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ContainsText_StringDoesNotContainTerm_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ContainsText("xyz");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsText_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ContainsText("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ContainsText Tests

    #region EndsWithText Tests

    [Fact]
    public void EndsWithText_StringEndsWith_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.EndsWithText("world");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EndsWithText_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.EndsWithText("WORLD");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EndsWithText_StringDoesNotEndWith_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.EndsWithText("hello");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EndsWithText_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.EndsWithText("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion EndsWithText Tests

    #region In Tests

    [Fact]
    public void In_ValueInCollection_ReturnsTrue()
    {
        // Arrange
        var value = "hello";
        var values = new[] { "hello", "world" };

        // Act
        var result = value.In(values);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void In_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var value = "HELLO";
        var values = new[] { "hello", "world" };

        // Act
        var result = value.In(values);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void In_ValueNotInCollection_ReturnsFalse()
    {
        // Arrange
        var value = "test";
        var values = new[] { "hello", "world" };

        // Act
        var result = value.In(values);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void In_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;
        var values = new[] { "hello", "world" };

        // Act
        var act = () => value!.In(values);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion In Tests

    #region IsLike Tests

    [Fact]
    public void IsLike_PatternWithPercentAtEnd_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.IsLike("hello%");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLike_PatternWithPercentAtStart_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.IsLike("%world");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLike_PatternWithUnderscore_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.IsLike("hello_world");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLike_PatternWithPercentInMiddle_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.IsLike("h%d");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLike_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.IsLike("HELLO%");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLike_PatternDoesNotMatch_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.IsLike("xyz%");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsLike_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.IsLike("test%");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion IsLike Tests

    #region StartsWithText Tests

    [Fact]
    public void StartsWithText_StringStartsWith_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.StartsWithText("hello");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void StartsWithText_CaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.StartsWithText("HELLO");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void StartsWithText_StringDoesNotStartWith_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.StartsWithText("world");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void StartsWithText_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.StartsWithText("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion StartsWithText Tests
}
