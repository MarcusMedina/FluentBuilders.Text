#pragma warning disable IDE0058 // Expression value is never used

namespace MarcusMedina.Fluent.Text.Tests.Extensions;

using FluentAssertions;
using MarcusMedina.Fluent.Text.Extensions.Manipulation;

public class StringManipulationExtensionsTests
{
    #region CollapseWhitespace Tests

    [Fact]
    public void CollapseWhitespace_MultipleSpaces_ReturnsSingleSpaces()
    {
        // Arrange
        var value = "hello    world  \t  test";

        // Act
        var result = value.CollapseWhitespace();

        // Assert
        result.Should().Be("hello world test");
    }

    [Fact]
    public void CollapseWhitespace_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CollapseWhitespace();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CollapseWhitespace Tests

    #region InsertAt Tests

    [Fact]
    public void InsertAt_ValidIndex_InsertsText()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.InsertAt(6, "beautiful ");

        // Assert
        result.Should().Be("hello beautiful world");
    }

    [Fact]
    public void InsertAt_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.InsertAt(0, "test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void InsertAt_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var value = "hello";

        // Act
        var act = () => value.InsertAt(100, "test");

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion InsertAt Tests

    #region Mask Tests

    [Fact]
    public void Mask_MiddleSection_MasksCorrectly()
    {
        // Arrange
        var value = "1234567890";

        // Act
        var result = value.Mask(4, 4);

        // Assert
        result.Should().Be("1234****90");
    }

    [Fact]
    public void Mask_CustomChar_UsesCustomMaskChar()
    {
        // Arrange
        var value = "password123";

        // Act
        var result = value.Mask(0, 8, '#');

        // Assert
        result.Should().Be("########123");
    }

    [Fact]
    public void Mask_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Mask(0, 5);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion Mask Tests

    #region RemoveWhitespace Tests

    [Fact]
    public void RemoveWhitespace_StringWithSpaces_RemovesAll()
    {
        // Arrange
        var value = "hello  world \t test";

        // Act
        var result = value.RemoveWhitespace();

        // Assert
        result.Should().Be("helloworldtest");
    }

    [Fact]
    public void RemoveWhitespace_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.RemoveWhitespace();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion RemoveWhitespace Tests

    #region Repeat Tests

    [Fact]
    public void Repeat_ThreeTimes_RepeatsCorrectly()
    {
        // Arrange
        var value = "Ha";

        // Act
        var result = value.Repeat(3);

        // Assert
        result.Should().Be("HaHaHa");
    }

    [Fact]
    public void Repeat_ZeroTimes_ReturnsEmpty()
    {
        // Arrange
        var value = "test";

        // Act
        var result = value.Repeat(0);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void Repeat_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Repeat(3);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Repeat_NegativeCount_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var value = "test";

        // Act
        var act = () => value.Repeat(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion Repeat Tests

    #region Reverse Tests

    [Fact]
    public void Reverse_HelloWorld_ReversesCorrectly()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.Reverse();

        // Assert
        result.Should().Be("olleh");
    }

    [Fact]
    public void Reverse_EmptyString_ReturnsEmpty()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.Reverse();

        // Assert
        result.Should().Be("");
    }

    [Fact]
    public void Reverse_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Reverse();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion Reverse Tests

    #region Shuffle Tests

    [Fact]
    public void Shuffle_String_ReturnsSameLength()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.Shuffle();

        // Assert
        result.Should().HaveLength(5);
    }

    [Fact]
    public void Shuffle_String_ContainsSameCharacters()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.Shuffle();

        // Assert
        result.Should()
              .Contain("h").And
              .Contain("e").And
              .Contain("l").And
              .Contain("o");
    }

    [Fact]
    public void Shuffle_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Shuffle();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion Shuffle Tests

    #region ShuffleSentences Tests

    [Fact]
    public void ShuffleSentences_String_ReturnsSameSentenceCount()
    {
        // Arrange
        var value = "Hello. How are you? I'm fine!";

        // Act
        var result = value.ShuffleSentences();

        // Assert
        result.Split(['.', '?', '!'], StringSplitOptions.RemoveEmptyEntries)
            .Should().HaveCount(3);
    }

    [Fact]
    public void ShuffleSentences_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ShuffleSentences();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ShuffleSentences Tests

    #region ShuffleWords Tests

    [Fact]
    public void ShuffleWords_String_ReturnsSameWordCount()
    {
        // Arrange
        var value = "hello world today";

        // Act
        var result = value.ShuffleWords();

        // Assert
        result.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Should().HaveCount(3);
    }

    [Fact]
    public void ShuffleWords_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ShuffleWords();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ShuffleWords Tests

    #region Truncate Tests

    [Fact]
    public void Truncate_LongString_TruncatesWithDefault()
    {
        // Arrange
        var value = "This is a long sentence";

        // Act
        var result = value.Truncate(10);

        // Assert
        result.Should().Be("This is...");
    }

    [Fact]
    public void Truncate_LongString_TruncatesWithCustomSuffix()
    {
        // Arrange
        var value = "This is a long sentence";

        // Act
        var result = value.Truncate(10, "~");

        // Assert
        result.Should().Be("This is a~");
    }

    [Fact]
    public void Truncate_ShortString_ReturnsOriginal()
    {
        // Arrange
        var value = "short";

        // Act
        var result = value.Truncate(10);

        // Assert
        result.Should().Be("short");
    }

    [Fact]
    public void Truncate_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Truncate(10);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion Truncate Tests

    #region WrapTextAt Tests

    [Fact]
    public void WrapTextAt_LongString_WrapsCorrectly()
    {
        // Arrange
        var value = "This is a very long sentence that needs wrapping";

        // Act
        var result = value.WrapTextAt(20, false);

        // Assert
        result.Should().Contain("\n");
        result.Split('\n').Should().AllSatisfy(line => line.Length.Should().BeLessThanOrEqualTo(20));
    }

    [Fact]
    public void WrapTextAt_ShortString_ReturnsOriginal()
    {
        // Arrange
        var value = "short";

        // Act
        var result = value.WrapTextAt(20);

        // Assert
        result.Should().Be("short");
    }

    [Fact]
    public void WrapTextAt_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.WrapTextAt(20);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WrapTextAt_ZeroMaxLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var value = "test";

        // Act
        var act = () => value.WrapTextAt(0);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion WrapTextAt Tests
}
