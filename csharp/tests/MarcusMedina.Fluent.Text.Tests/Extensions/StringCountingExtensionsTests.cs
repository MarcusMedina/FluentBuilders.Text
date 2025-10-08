#pragma warning disable IDE0058 // Expression value is never used

namespace MarcusMedina.Fluent.Text.Tests.Extensions;

using FluentAssertions;
using MarcusMedina.Fluent.Text.Extensions.Counting;

public class StringCountingExtensionsTests
{
    #region CountConsonants Tests

    [Fact]
    public void CountConsonants_HelloWorld_Returns7()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var result = value.CountConsonants();

        // Assert
        result.Should().Be(7);
    }

    [Fact]
    public void CountConsonants_OnlyVowels_Returns0()
    {
        // Arrange
        var value = "aeiou";

        // Act
        var result = value.CountConsonants();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountConsonants_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountConsonants();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountConsonants Tests

    #region CountDigits Tests

    [Fact]
    public void CountDigits_MixedString_Returns6()
    {
        // Arrange
        var value = "abc123def456";

        // Act
        var result = value.CountDigits();

        // Assert
        result.Should().Be(6);
    }

    [Fact]
    public void CountDigits_NoDigits_Returns0()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.CountDigits();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountDigits_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountDigits();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountDigits Tests

    #region CountLetters Tests

    [Fact]
    public void CountLetters_MixedString_Returns10()
    {
        // Arrange
        var value = "Hello World 123!";

        // Act
        var result = value.CountLetters();

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public void CountLetters_NoLetters_Returns0()
    {
        // Arrange
        var value = "123 !@#";

        // Act
        var result = value.CountLetters();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountLetters_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountLetters();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountLetters Tests

    #region CountLines Tests

    [Fact]
    public void CountLines_ThreeLines_Returns3()
    {
        // Arrange
        var value = "line1\nline2\nline3";

        // Act
        var result = value.CountLines();

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void CountLines_EmptyString_Returns0()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.CountLines();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountLines_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountLines();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountLines Tests

    #region CountLowercase Tests

    [Fact]
    public void CountLowercase_HelloWorld_Returns8()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var result = value.CountLowercase();

        // Assert
        result.Should().Be(8);
    }

    [Fact]
    public void CountLowercase_AllUppercase_Returns0()
    {
        // Arrange
        var value = "HELLO";

        // Act
        var result = value.CountLowercase();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountLowercase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountLowercase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountLowercase Tests

    #region CountOccurrences Character Tests

    [Fact]
    public void CountOccurrences_Char_Returns3()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.CountOccurrences('l');

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void CountOccurrences_Char_CaseInsensitive_Returns2()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var result = value.CountOccurrences('o', false);

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public void CountOccurrences_Char_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountOccurrences('a');

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountOccurrences Character Tests

    #region CountOccurrences String Tests

    [Fact]
    public void CountOccurrences_String_Returns2()
    {
        // Arrange
        var value = "hello hello world";

        // Act
        var result = value.CountOccurrences("hello");

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public void CountOccurrences_String_CaseInsensitive_Returns3()
    {
        // Arrange
        var value = "Hello HELLO hello";

        // Act
        var result = value.CountOccurrences("hello", false);

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void CountOccurrences_String_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountOccurrences("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountOccurrences String Tests

    #region CountSentences Tests

    [Fact]
    public void CountSentences_ThreeSentences_Returns3()
    {
        // Arrange
        var value = "Hello. How are you? I'm fine!";

        // Act
        var result = value.CountSentences();

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void CountSentences_EmptyString_Returns0()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.CountSentences();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountSentences_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountSentences();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountSentences Tests

    #region CountUppercase Tests

    [Fact]
    public void CountUppercase_HelloWorld_Returns2()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var result = value.CountUppercase();

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public void CountUppercase_AllLowercase_Returns0()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.CountUppercase();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountUppercase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountUppercase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountUppercase Tests

    #region CountVowels Tests

    [Fact]
    public void CountVowels_HelloWorld_Returns3()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var result = value.CountVowels();

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void CountVowels_NoVowels_Returns0()
    {
        // Arrange
        var value = "bcdfg";

        // Act
        var result = value.CountVowels();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountVowels_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountVowels();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountVowels Tests

    #region CountWords Tests

    [Fact]
    public void CountWords_FiveWords_Returns5()
    {
        // Arrange
        var value = "Hello world, how are you?";

        // Act
        var result = value.CountWords();

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void CountWords_EmptyString_Returns0()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.CountWords();

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void CountWords_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.CountWords();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion CountWords Tests
}
