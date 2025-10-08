namespace MarcusMedina.Fluent.Text.Tests.Extensions;

using FluentAssertions;
using MarcusMedina.Fluent.Text.Extensions.Casing;

public class StringCasingExtensionsTests
{
    #region ToUpperCase Tests

    [Fact]
    public void ToUpperCase_LowercaseString_ReturnsUppercase()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.ToUpperCase();

        // Assert
        result.Should().Be("HELLO");
    }

    [Fact]
    public void ToUpperCase_MixedCaseString_ReturnsUppercase()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var result = value.ToUpperCase();

        // Assert
        result.Should().Be("HELLO WORLD");
    }

    [Fact]
    public void ToUpperCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToUpperCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion ToUpperCase Tests

    #region ToLowerCase Tests

    [Fact]
    public void ToLowerCase_MixedCaseString_ReturnsLowercase()
    {
        // Arrange
        var value = "Hello World";

        // Act
        var result = value.ToLowerCase();

        // Assert
        result.Should().Be("hello world");
    }

    [Fact]
    public void ToLowerCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToLowerCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToLowerCase_UppercaseString_ReturnsLowercase()
    {
        // Arrange
        var value = "HELLO";

        // Act
        var result = value.ToLowerCase();

        // Assert
        result.Should().Be("hello");
    }

    #endregion ToLowerCase Tests

    #region ToTitleCase Tests

    [Fact]
    public void ToTitleCase_LowercaseString_ReturnsTitleCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToTitleCase();

        // Assert
        result.Should().Be("Hello World");
    }

    [Fact]
    public void ToTitleCase_MixedCaseString_ReturnsTitleCase()
    {
        // Arrange
        var value = "hELLo WoRLd";

        // Act
        var result = value.ToTitleCase();

        // Assert
        result.Should().Be("Hello World");
    }

    [Fact]
    public void ToTitleCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToTitleCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToTitleCase_UppercaseString_ReturnsTitleCase()
    {
        // Arrange
        var value = "HELLO WORLD";

        // Act
        var result = value.ToTitleCase();

        // Assert
        result.Should().Be("Hello World");
    }

    #endregion ToTitleCase Tests

    #region ToSentenceCase Tests

    [Fact]
    public void ToSentenceCase_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.ToSentenceCase();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ToSentenceCase_LowercaseString_ReturnsSentenceCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToSentenceCase();

        // Assert
        result.Should().Be("Hello world");
    }

    [Fact]
    public void ToSentenceCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToSentenceCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToSentenceCase_SingleCharacter_ReturnsUppercase()
    {
        // Arrange
        var value = "h";

        // Act
        var result = value.ToSentenceCase();

        // Assert
        result.Should().Be("H");
    }

    [Fact]
    public void ToSentenceCase_UppercaseString_ReturnsSentenceCase()
    {
        // Arrange
        var value = "HELLO WORLD";

        // Act
        var result = value.ToSentenceCase();

        // Assert
        result.Should().Be("HELLO WORLD");
    }

    #endregion ToSentenceCase Tests

    #region ToProperCase Tests

    [Fact]
    public void ToProperCase_IsAliasForToTitleCase()
    {
        // Arrange
        var value = "test string";

        // Act
        var properCase = value.ToProperCase();
        var titleCase = value.ToTitleCase();

        // Assert
        properCase.Should().Be(titleCase);
    }

    [Fact]
    public void ToProperCase_LowercaseString_ReturnsProperCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToProperCase();

        // Assert
        result.Should().Be("Hello World");
    }

    #endregion ToProperCase Tests

    #region ToNameCase Tests

    [Fact]
    public void ToNameCase_ComplexName_ReturnsCorrectCase()
    {
        // Arrange
        var value = "jean-claude van damme";

        // Act
        var result = value.ToNameCase();

        // Assert
        result.Should().Be("Jean-Claude van Damme");
    }

    [Fact]
    public void ToNameCase_HyphenatedName_ReturnsCorrectCase()
    {
        // Arrange
        var value = "mary-jane";

        // Act
        var result = value.ToNameCase();

        // Assert
        result.Should().Be("Mary-Jane");
    }

    [Fact]
    public void ToNameCase_MacPrefix_ReturnsCorrectCase()
    {
        // Arrange
        var value = "macarthur";

        // Act
        var result = value.ToNameCase();

        // Assert
        result.Should().Be("MacArthur");
    }

    [Fact]
    public void ToNameCase_McPrefix_ReturnsCorrectCase()
    {
        // Arrange
        var value = "mcdonald";

        // Act
        var result = value.ToNameCase();

        // Assert
        result.Should().Be("McDonald");
    }

    [Fact]
    public void ToNameCase_NameParticle_ReturnsCorrectCase()
    {
        // Arrange
        var value = "von neumann";

        // Act
        var result = value.ToNameCase();

        // Assert
        result.Should().Be("von Neumann");
    }

    [Fact]
    public void ToNameCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToNameCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToNameCase_RomanNumeral_ReturnsCorrectCase()
    {
        // Arrange
        var value = "henry viii";

        // Act
        var result = value.ToNameCase();

        // Assert
        result.Should().Be("Henry VIII");
    }

    [Fact]
    public void ToNameCase_SimpleApostrophe_ReturnsCorrectCase()
    {
        // Arrange
        var value = "o'brien";

        // Act
        var result = value.ToNameCase();

        // Assert
        result.Should().Be("O'Brien");
    }

    #endregion ToNameCase Tests

    #region ToPascalCase Tests

    [Fact]
    public void ToPascalCase_CamelCaseInput_ReturnsPascalCase()
    {
        // Arrange
        var value = "helloWorld";

        // Act
        var result = value.ToPascalCase();

        // Assert
        result.Should().Be("HelloWorld");
    }

    [Fact]
    public void ToPascalCase_HyphenSeparatedWords_ReturnsPascalCase()
    {
        // Arrange
        var value = "hello-world";

        // Act
        var result = value.ToPascalCase();

        // Assert
        result.Should().Be("HelloWorld");
    }

    [Fact]
    public void ToPascalCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToPascalCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToPascalCase_SpaceSeparatedWords_ReturnsPascalCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToPascalCase();

        // Assert
        result.Should().Be("HelloWorld");
    }

    [Fact]
    public void ToPascalCase_UnderscoreSeparatedWords_ReturnsPascalCase()
    {
        // Arrange
        var value = "hello_world";

        // Act
        var result = value.ToPascalCase();

        // Assert
        result.Should().Be("HelloWorld");
    }

    #endregion ToPascalCase Tests

    #region ToCamelCase Tests

    [Fact]
    public void ToCamelCase_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.ToCamelCase();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ToCamelCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToCamelCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToCamelCase_PascalCaseInput_ReturnsCamelCase()
    {
        // Arrange
        var value = "HelloWorld";

        // Act
        var result = value.ToCamelCase();

        // Assert
        result.Should().Be("helloWorld");
    }

    [Fact]
    public void ToCamelCase_SpaceSeparatedWords_ReturnsCamelCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToCamelCase();

        // Assert
        result.Should().Be("helloWorld");
    }

    #endregion ToCamelCase Tests

    #region ToKebabCase Tests

    [Fact]
    public void ToKebabCase_CamelCaseInput_ReturnsKebabCase()
    {
        // Arrange
        var value = "helloWorld";

        // Act
        var result = value.ToKebabCase();

        // Assert
        result.Should().Be("hello-world");
    }

    [Fact]
    public void ToKebabCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToKebabCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToKebabCase_PascalCaseInput_ReturnsKebabCase()
    {
        // Arrange
        var value = "HelloWorld";

        // Act
        var result = value.ToKebabCase();

        // Assert
        result.Should().Be("hello-world");
    }

    [Fact]
    public void ToKebabCase_SpaceSeparatedWords_ReturnsKebabCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToKebabCase();

        // Assert
        result.Should().Be("hello-world");
    }

    #endregion ToKebabCase Tests

    #region ToSnakeCase Tests

    [Fact]
    public void ToSnakeCase_CamelCaseInput_ReturnsSnakeCase()
    {
        // Arrange
        var value = "helloWorld";

        // Act
        var result = value.ToSnakeCase();

        // Assert
        result.Should().Be("hello_world");
    }

    [Fact]
    public void ToSnakeCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToSnakeCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToSnakeCase_PascalCaseInput_ReturnsSnakeCase()
    {
        // Arrange
        var value = "HelloWorld";

        // Act
        var result = value.ToSnakeCase();

        // Assert
        result.Should().Be("hello_world");
    }

    [Fact]
    public void ToSnakeCase_SpaceSeparatedWords_ReturnsSnakeCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToSnakeCase();

        // Assert
        result.Should().Be("hello_world");
    }

    #endregion ToSnakeCase Tests

    #region ToScreamingSnakeCase Tests

    [Fact]
    public void ToScreamingSnakeCase_CamelCaseInput_ReturnsScreamingSnakeCase()
    {
        // Arrange
        var value = "helloWorld";

        // Act
        var result = value.ToScreamingSnakeCase();

        // Assert
        result.Should().Be("HELLO_WORLD");
    }

    [Fact]
    public void ToScreamingSnakeCase_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToScreamingSnakeCase();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToScreamingSnakeCase_PascalCaseInput_ReturnsScreamingSnakeCase()
    {
        // Arrange
        var value = "HelloWorld";

        // Act
        var result = value.ToScreamingSnakeCase();

        // Assert
        result.Should().Be("HELLO_WORLD");
    }

    [Fact]
    public void ToScreamingSnakeCase_SpaceSeparatedWords_ReturnsScreamingSnakeCase()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToScreamingSnakeCase();

        // Assert
        result.Should().Be("HELLO_WORLD");
    }

    #endregion ToScreamingSnakeCase Tests
}