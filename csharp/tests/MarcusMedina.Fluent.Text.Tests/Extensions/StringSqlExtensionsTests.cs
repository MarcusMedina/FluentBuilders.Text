namespace MarcusMedina.Fluent.Text.Tests.Extensions;
#pragma warning disable IDE0058 // Expression value is never used

using MarcusMedina.Fluent.Text.Extensions.Pattern;
using FluentAssertions;
using Xunit;

public class StringSqlExtensionsTests
{
    #region IsLike Tests

    [Fact]
    public void IsLike_CaseInsensitive_MatchesIgnoringCase()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.IsLike("HELLO%").Should().BeTrue();
        value.IsLike("%WORLD").Should().BeTrue();
    }

    [Fact]
    public void IsLike_CaseSensitive_MatchesRespectingCase()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.IsLike("HELLO%", caseSensitive: true).Should().BeFalse();
        value.IsLike("hello%", caseSensitive: true).Should().BeTrue();
    }

    [Fact]
    public void IsLike_NoMatch_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.IsLike("xyz%").Should().BeFalse();
        value.IsLike("%xyz").Should().BeFalse();
    }

    [Fact]
    public void IsLike_NullPattern_ThrowsArgumentNullException()
    {
        // Arrange
        var value = "hello";

        // Act
        var act = () => value.IsLike(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsLike_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.IsLike("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsLike_PercentWildcard_MatchesZeroOrMoreCharacters()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.IsLike("hello%").Should().BeTrue();
        value.IsLike("%world").Should().BeTrue();
        value.IsLike("%").Should().BeTrue();
        value.IsLike("h%d").Should().BeTrue();
    }

    [Fact]
    public void IsLike_UnderscoreWildcard_MatchesExactlyOneCharacter()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.IsLike("hello_world").Should().BeTrue();
        value.IsLike("h____").Should().BeFalse();
        value.IsLike("h_____%").Should().BeTrue();
    }

    #endregion IsLike Tests

    #region ContainsText Tests

    [Fact]
    public void ContainsText_CaseInsensitive_MatchesIgnoringCase()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.ContainsText("WORLD").Should().BeTrue();
        value.ContainsText("HELLO").Should().BeTrue();
    }

    [Fact]
    public void ContainsText_CaseSensitive_MatchesRespectingCase()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.ContainsText("WORLD", caseSensitive: true).Should().BeFalse();
        value.ContainsText("world", caseSensitive: true).Should().BeTrue();
    }

    [Fact]
    public void ContainsText_ExistingSubstring_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.ContainsText("world").Should().BeTrue();
        value.ContainsText("hello").Should().BeTrue();
        value.ContainsText("lo wo").Should().BeTrue();
    }

    [Fact]
    public void ContainsText_NonExistingSubstring_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.ContainsText("xyz").Should().BeFalse();
    }

    [Fact]
    public void ContainsText_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ContainsText("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ContainsText Tests

    #region StartsWithText Tests

    [Fact]
    public void StartsWithText_CaseInsensitive_MatchesIgnoringCase()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.StartsWithText("HELLO").Should().BeTrue();
    }

    [Fact]
    public void StartsWithText_CaseSensitive_MatchesRespectingCase()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.StartsWithText("HELLO", caseSensitive: true).Should().BeFalse();
        value.StartsWithText("hello", caseSensitive: true).Should().BeTrue();
    }

    [Fact]
    public void StartsWithText_CorrectPrefix_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.StartsWithText("hello").Should().BeTrue();
        value.StartsWithText("h").Should().BeTrue();
    }

    [Fact]
    public void StartsWithText_IncorrectPrefix_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.StartsWithText("world").Should().BeFalse();
    }

    [Fact]
    public void StartsWithText_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.StartsWithText("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion StartsWithText Tests

    #region EndsWithText Tests

    [Fact]
    public void EndsWithText_CaseInsensitive_MatchesIgnoringCase()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.EndsWithText("WORLD").Should().BeTrue();
    }

    [Fact]
    public void EndsWithText_CorrectSuffix_ReturnsTrue()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.EndsWithText("world").Should().BeTrue();
        value.EndsWithText("d").Should().BeTrue();
    }

    [Fact]
    public void EndsWithText_IncorrectSuffix_ReturnsFalse()
    {
        // Arrange
        var value = "hello world";

        // Act & Assert
        value.EndsWithText("hello").Should().BeFalse();
    }

    [Fact]
    public void EndsWithText_NullValue_ThrowsArgumentNullException()
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
    public void In_CaseInsensitive_MatchesIgnoringCase()
    {
        // Arrange
        var value = "HELLO";
        var values = new[] { "hello", "world" };

        // Act & Assert
        value.In(values).Should().BeTrue();
    }

    [Fact]
    public void In_CaseSensitive_MatchesRespectingCase()
    {
        // Arrange
        var value = "HELLO";
        var values = new[] { "hello", "world" };

        // Act & Assert
        value.In(values, caseSensitive: true).Should().BeFalse();
    }

    [Fact]
    public void In_EmptyCollection_ReturnsFalse()
    {
        // Arrange
        var value = "hello";
        var values = Array.Empty<string>();

        // Act & Assert
        value.In(values).Should().BeFalse();
    }

    [Fact]
    public void In_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;
        var values = new[] { "test" };

        // Act
        var act = () => value!.In(values);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void In_ValueInCollection_ReturnsTrue()
    {
        // Arrange
        var value = "hello";
        var values = new[] { "hello", "world", "test" };

        // Act & Assert
        value.In(values).Should().BeTrue();
    }

    [Fact]
    public void In_ValueNotInCollection_ReturnsFalse()
    {
        // Arrange
        var value = "xyz";
        var values = new[] { "hello", "world" };

        // Act & Assert
        value.In(values).Should().BeFalse();
    }

    #endregion In Tests

    #region Between Tests

    [Fact]
    public void Between_CaseInsensitive_MatchesIgnoringCase()
    {
        // Arrange
        var value = "BOB";

        // Act & Assert
        value.Between("alice", "charlie").Should().BeTrue();
    }

    [Fact]
    public void Between_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Between("a", "z");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Between_ValueAboveRange_ReturnsFalse()
    {
        // Arrange
        var value = "dave";

        // Act & Assert
        value.Between("alice", "charlie").Should().BeFalse();
    }

    [Fact]
    public void Between_ValueAtEndBoundary_ReturnsTrue()
    {
        // Arrange
        var value = "charlie";

        // Act & Assert
        value.Between("alice", "charlie").Should().BeTrue();
    }

    [Fact]
    public void Between_ValueAtStartBoundary_ReturnsTrue()
    {
        // Arrange
        var value = "alice";

        // Act & Assert
        value.Between("alice", "charlie").Should().BeTrue();
    }

    [Fact]
    public void Between_ValueBelowRange_ReturnsFalse()
    {
        // Arrange
        var value = "adam";

        // Act & Assert
        value.Between("alice", "charlie").Should().BeFalse();
    }

    [Fact]
    public void Between_ValueInRange_ReturnsTrue()
    {
        // Arrange
        var value = "bob";

        // Act & Assert
        value.Between("alice", "charlie").Should().BeTrue();
    }

    #endregion Between Tests
}