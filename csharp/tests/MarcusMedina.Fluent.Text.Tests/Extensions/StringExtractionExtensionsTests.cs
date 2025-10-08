namespace MarcusMedina.Fluent.Text.Tests.Extensions;
#pragma warning disable IDE0058 // Expression value is never used

using MarcusMedina.Fluent.Text.Extensions.Extraction;
using FluentAssertions;
using Xunit;

public class StringExtractionExtensionsTests
{
    #region ExtractAllSentences Tests

    [Fact]
    public void ExtractAllSentences_MultipleSentences_ReturnsArray()
    {
        // Arrange
        var value = "Hello. How are you? I'm fine!";

        // Act
        var result = value.ExtractAllSentences();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain("Hello.");
    }

    [Fact]
    public void ExtractAllSentences_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractAllSentences();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractAllSentences Tests

    #region ExtractAllWords Tests

    [Fact]
    public void ExtractAllWords_StringWithWords_ReturnsArray()
    {
        // Arrange
        var value = "Hello world, how are you?";

        // Act
        var result = value.ExtractAllWords();

        // Assert
        result.Should().HaveCount(5);
        result.Should().Contain("Hello");
        result.Should().Contain("world");
    }

    [Fact]
    public void ExtractAllWords_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractAllWords();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractAllWords Tests

    #region ExtractBetween Tests

    [Fact]
    public void ExtractBetween_ValidMarkers_ReturnsContent()
    {
        // Arrange
        var value = "Hello [world] and [test]";

        // Act
        var result = value.ExtractBetween("[", "]");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("world");
        result.Should().Contain("test");
    }

    [Fact]
    public void ExtractBetween_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractBetween("[", "]");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractBetween Tests

    #region ExtractDates Tests

    [Fact]
    public void ExtractDates_StringWithDates_ReturnsDates()
    {
        // Arrange
        var value = "Meeting on 2024-01-15 and 2024-12-25";

        // Act
        var result = value.ExtractDates();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void ExtractDates_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractDates();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractDates Tests

    #region ExtractEmails Tests

    [Fact]
    public void ExtractEmails_StringWithEmails_ReturnsEmails()
    {
        // Arrange
        var value = "Contact us at test@example.com or admin@example.org";

        // Act
        var result = value.ExtractEmails();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("test@example.com");
        result.Should().Contain("admin@example.org");
    }

    [Fact]
    public void ExtractEmails_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractEmails();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractEmails Tests

    #region ExtractHashtags Tests

    [Fact]
    public void ExtractHashtags_StringWithHashtags_ReturnsHashtags()
    {
        // Arrange
        var value = "Love #coding and #programming!";

        // Act
        var result = value.ExtractHashtags();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("#coding");
        result.Should().Contain("#programming");
    }

    [Fact]
    public void ExtractHashtags_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractHashtags();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractHashtags Tests

    #region ExtractMentions Tests

    [Fact]
    public void ExtractMentions_StringWithMentions_ReturnsMentions()
    {
        // Arrange
        var value = "Hey @john and @mary!";

        // Act
        var result = value.ExtractMentions();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("@john");
        result.Should().Contain("@mary");
    }

    [Fact]
    public void ExtractMentions_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractMentions();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractMentions Tests

    #region ExtractNumbers Tests

    [Fact]
    public void ExtractNumbers_StringWithNumbers_ReturnsNumbers()
    {
        // Arrange
        var value = "I have 42 apples and 123 oranges";

        // Act
        var result = value.ExtractNumbers();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("42");
        result.Should().Contain("123");
    }

    [Fact]
    public void ExtractNumbers_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractNumbers();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractNumbers Tests

    #region ExtractPhoneNumbers Tests

    [Fact]
    public void ExtractPhoneNumbers_StringWithPhones_ReturnsPhones()
    {
        // Arrange
        var value = "Call 123-456-7890 or (555) 123-4567";

        // Act
        var result = value.ExtractPhoneNumbers();

        // Assert
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void ExtractPhoneNumbers_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractPhoneNumbers();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractPhoneNumbers Tests

    #region ExtractUrls Tests

    [Fact]
    public void ExtractUrls_StringWithUrls_ReturnsUrls()
    {
        // Arrange
        var value = "Visit https://example.com or http://test.org";

        // Act
        var result = value.ExtractUrls();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("https://example.com");
        result.Should().Contain("http://test.org");
    }

    [Fact]
    public void ExtractUrls_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractUrls();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractUrls Tests

    #region ExtractWordsContaining Tests

    [Fact]
    public void ExtractWordsContaining_ValidSubstring_ReturnsWords()
    {
        // Arrange
        var value = "hello world work place";

        // Act
        var result = value.ExtractWordsContaining("or");

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("world");
        result.Should().Contain("work");
    }

    [Fact]
    public void ExtractWordsContaining_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractWordsContaining("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractWordsContaining Tests

    #region ExtractWordsEndingWith Tests

    [Fact]
    public void ExtractWordsEndingWith_ValidSuffix_ReturnsWords()
    {
        // Arrange
        var value = "running jumping walking";

        // Act
        var result = value.ExtractWordsEndingWith("ing");

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain("running");
    }

    [Fact]
    public void ExtractWordsEndingWith_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractWordsEndingWith("test");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractWordsEndingWith Tests

    #region ExtractWordsOfLength Tests

    [Fact]
    public void ExtractWordsOfLength_ValidLength_ReturnsWords()
    {
        // Arrange
        var value = "I am running fast today";

        // Act
        var result = value.ExtractWordsOfLength(5);

        // Assert
        result.Should().Contain("today");
    }

    [Fact]
    public void ExtractWordsOfLength_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractWordsOfLength(5);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractWordsOfLength Tests

    #region ExtractWordsStartingWith Tests

    [Fact]
    public void ExtractWordsStartingWith_ValidPrefix_ReturnsWords()
    {
        // Arrange
        var value = "hello happy house world";

        // Act
        var result = value.ExtractWordsStartingWith("h");

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain("hello");
        result.Should().Contain("happy");
    }

    [Fact]
    public void ExtractWordsStartingWith_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ExtractWordsStartingWith("h");

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ExtractWordsStartingWith Tests

    #region Left Tests

    [Fact]
    public void Left_ValidLength_ReturnsLeftChars()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.Left(5);

        // Assert
        result.Should().Be("hello");
    }

    [Fact]
    public void Left_LengthExceedsString_ReturnsFullString()
    {
        // Arrange
        var value = "hi";

        // Act
        var result = value.Left(5);

        // Assert
        result.Should().Be("hi");
    }

    [Fact]
    public void Left_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Left(5);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Left_NegativeLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var value = "test";

        // Act
        var act = () => value.Left(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion Left Tests

    #region Mid Tests

    [Fact]
    public void Mid_ValidStartAndLength_ReturnsSubstring()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.Mid(6, 5);

        // Assert
        result.Should().Be("world");
    }

    [Fact]
    public void Mid_StartAtBeginning_ReturnsSubstring()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.Mid(0, 2);

        // Assert
        result.Should().Be("he");
    }

    [Fact]
    public void Mid_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Mid(0, 5);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Mid_NegativeStart_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var value = "test";

        // Act
        var act = () => value.Mid(-1, 2);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion Mid Tests

    #region Right Tests

    [Fact]
    public void Right_ValidLength_ReturnsRightChars()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.Right(5);

        // Assert
        result.Should().Be("world");
    }

    [Fact]
    public void Right_LengthExceedsString_ReturnsFullString()
    {
        // Arrange
        var value = "hi";

        // Act
        var result = value.Right(5);

        // Assert
        result.Should().Be("hi");
    }

    [Fact]
    public void Right_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.Right(5);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Right_NegativeLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var value = "test";

        // Act
        var act = () => value.Right(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    #endregion Right Tests
}