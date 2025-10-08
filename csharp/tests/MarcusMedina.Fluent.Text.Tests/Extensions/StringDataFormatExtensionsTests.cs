namespace MarcusMedina.Fluent.Text.Tests.Extensions;
#pragma warning disable IDE0058 // Expression value is never used

using MarcusMedina.Fluent.Text.Extensions.DataFormat;
using FluentAssertions;
using Xunit;

public class StringDataFormatExtensionsTests
{
    #region ToCsvField Tests

    [Fact]
    public void ToCsvField_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.ToCsvField();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ToCsvField_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToCsvField();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToCsvField_SimpleValue_ReturnsUnquoted()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.ToCsvField();

        // Assert
        result.Should().Be("hello");
    }

    [Fact]
    public void ToCsvField_ValueWithComma_ReturnsQuoted()
    {
        // Arrange
        var value = "hello, world";

        // Act
        var result = value.ToCsvField();

        // Assert
        result.Should().Be("\"hello, world\"");
    }

    [Fact]
    public void ToCsvField_ValueWithNewline_ReturnsQuoted()
    {
        // Arrange
        var value = "line1\nline2";

        // Act
        var result = value.ToCsvField();

        // Assert
        result.Should().Be("\"line1\nline2\"");
    }

    [Fact]
    public void ToCsvField_ValueWithQuotes_EscapesQuotes()
    {
        // Arrange
        var value = "say \"hello\"";

        // Act
        var result = value.ToCsvField();

        // Assert
        result.Should().Be("\"say \"\"hello\"\"\"");
    }

    [Fact]
    public void ToCsvField_ValueWithSemicolon_ReturnsQuotedWhenDelimiterIsSemicolon()
    {
        // Arrange
        var value = "value;data";

        // Act
        var result = value.ToCsvField(';');

        // Assert
        result.Should().Be("\"value;data\"");
    }

    #endregion ToCsvField Tests

    #region FromCsvField Tests

    [Fact]
    public void FromCsvField_EscapedQuotes_UnescapesQuotes()
    {
        // Arrange
        var value = "\"say \"\"hello\"\"\"";

        // Act
        var result = value.FromCsvField();

        // Assert
        result.Should().Be("say \"hello\"");
    }

    [Fact]
    public void FromCsvField_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.FromCsvField();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FromCsvField_QuotedValue_RemovesQuotes()
    {
        // Arrange
        var value = "\"hello, world\"";

        // Act
        var result = value.FromCsvField();

        // Assert
        result.Should().Be("hello, world");
    }

    [Fact]
    public void FromCsvField_SimpleValue_ReturnsUnchanged()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.FromCsvField();

        // Assert
        result.Should().Be("hello");
    }

    #endregion FromCsvField Tests

    #region SplitCsvLine Tests

    [Fact]
    public void SplitCsvLine_ComplexExample_ParsesCorrectly()
    {
        // Arrange
        var value = "Name,\"Age, Years\",\"Quote: \"\"Hi\"\"\",City";

        // Act
        var result = value.SplitCsvLine();

        // Assert
        result.Should().Equal("Name", "Age, Years", "Quote: \"Hi\"", "City");
    }

    [Fact]
    public void SplitCsvLine_EmptyFields_PreservesEmptyStrings()
    {
        // Arrange
        var value = "a,,c";

        // Act
        var result = value.SplitCsvLine();

        // Assert
        result.Should().Equal("a", "", "c");
    }

    [Fact]
    public void SplitCsvLine_EscapedQuotes_UnescapesCorrectly()
    {
        // Arrange
        var value = "\"say \"\"hi\"\"\",data";

        // Act
        var result = value.SplitCsvLine();

        // Assert
        result.Should().Equal("say \"hi\"", "data");
    }

    [Fact]
    public void SplitCsvLine_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.SplitCsvLine();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SplitCsvLine_QuotedValue_PreservesCommas()
    {
        // Arrange
        var value = "\"hello, world\",test";

        // Act
        var result = value.SplitCsvLine();

        // Assert
        result.Should().Equal("hello, world", "test");
    }

    [Fact]
    public void SplitCsvLine_SemicolonDelimiter_SplitsCorrectly()
    {
        // Arrange
        var value = "a;b;c";

        // Act
        var result = value.SplitCsvLine(';');

        // Assert
        result.Should().Equal("a", "b", "c");
    }

    [Fact]
    public void SplitCsvLine_SimpleValues_SplitsCorrectly()
    {
        // Arrange
        var value = "a,b,c";

        // Act
        var result = value.SplitCsvLine();

        // Assert
        result.Should().Equal("a", "b", "c");
    }

    #endregion SplitCsvLine Tests

    #region ToCsvLine Tests

    [Fact]
    public void ToCsvLine_NullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<string>? values = null;

        // Act
        var act = () => values!.ToCsvLine();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToCsvLine_SemicolonDelimiter_JoinsWithSemicolon()
    {
        // Arrange
        var values = new[] { "a", "b", "c" };

        // Act
        var result = values.ToCsvLine(';');

        // Assert
        result.Should().Be("a;b;c");
    }

    [Fact]
    public void ToCsvLine_SimpleValues_JoinsWithComma()
    {
        // Arrange
        var values = new[] { "a", "b", "c" };

        // Act
        var result = values.ToCsvLine();

        // Assert
        result.Should().Be("a,b,c");
    }

    [Fact]
    public void ToCsvLine_ValuesWithCommas_QuotesAutomatically()
    {
        // Arrange
        var values = new[] { "hello, world", "test" };

        // Act
        var result = values.ToCsvLine();

        // Assert
        result.Should().Be("\"hello, world\",test");
    }

    [Fact]
    public void ToCsvLine_ValuesWithQuotes_EscapesCorrectly()
    {
        // Arrange
        var values = new[] { "say \"hi\"", "data" };

        // Act
        var result = values.ToCsvLine();

        // Assert
        result.Should().Be("\"say \"\"hi\"\"\",data");
    }

    #endregion ToCsvLine Tests

    #region ToJsonString Tests

    [Fact]
    public void ToJsonString_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToJsonString();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToJsonString_SimpleValue_ReturnsUnchanged()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.ToJsonString();

        // Assert
        result.Should().Be("hello");
    }

    [Fact]
    public void ToJsonString_ValueWithBackslash_EscapesBackslash()
    {
        // Arrange
        var value = "path\\file";

        // Act
        var result = value.ToJsonString();

        // Assert
        result.Should().Be("path\\\\file");
    }

    [Fact]
    public void ToJsonString_ValueWithCarriageReturn_EscapesCarriageReturn()
    {
        // Arrange
        var value = "line1\rline2";

        // Act
        var result = value.ToJsonString();

        // Assert
        result.Should().Be("line1\\rline2");
    }

    [Fact]
    public void ToJsonString_ValueWithNewline_EscapesNewline()
    {
        // Arrange
        var value = "line1\nline2";

        // Act
        var result = value.ToJsonString();

        // Assert
        result.Should().Be("line1\\nline2");
    }

    [Fact]
    public void ToJsonString_ValueWithQuotes_EscapesQuotes()
    {
        // Arrange
        var value = "say \"hi\"";

        // Act
        var result = value.ToJsonString();

        // Assert
        result.Should().Be("say \\\"hi\\\"");
    }

    [Fact]
    public void ToJsonString_ValueWithTab_EscapesTab()
    {
        // Arrange
        var value = "col1\tcol2";

        // Act
        var result = value.ToJsonString();

        // Assert
        result.Should().Be("col1\\tcol2");
    }

    #endregion ToJsonString Tests

    #region FromJsonString Tests

    [Fact]
    public void FromJsonString_EscapedBackslash_UnescapesBackslash()
    {
        // Arrange
        var value = "path\\\\file";

        // Act
        var result = value.FromJsonString();

        // Assert
        result.Should().Be("path\\file");
    }

    [Fact]
    public void FromJsonString_EscapedNewline_UnescapesNewline()
    {
        // Arrange
        var value = "line1\\nline2";

        // Act
        var result = value.FromJsonString();

        // Assert
        result.Should().Be("line1\nline2");
    }

    [Fact]
    public void FromJsonString_EscapedQuotes_UnescapesQuotes()
    {
        // Arrange
        var value = "say \\\"hi\\\"";

        // Act
        var result = value.FromJsonString();

        // Assert
        result.Should().Be("say \"hi\"");
    }

    [Fact]
    public void FromJsonString_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.FromJsonString();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FromJsonString_RoundTrip_PreservesOriginal()
    {
        // Arrange
        var original = "say \"hello\"\nworld\\path";

        // Act
        var escaped = original.ToJsonString();
        var unescaped = escaped.FromJsonString();

        // Assert
        unescaped.Should().Be(original);
    }

    [Fact]
    public void FromJsonString_SimpleValue_ReturnsUnchanged()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.FromJsonString();

        // Assert
        result.Should().Be("hello");
    }

    #endregion FromJsonString Tests

    #region IsValidJson Tests

    [Fact]
    public void IsValidJson_EmptyString_ReturnsFalse()
    {
        // Arrange
        var value = "";

        // Act
        var result = value.IsValidJson();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidJson_InvalidJson_ReturnsFalse()
    {
        // Arrange
        var value = "not json";

        // Act
        var result = value.IsValidJson();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidJson_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.IsValidJson();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsValidJson_ValidArray_ReturnsTrue()
    {
        // Arrange
        var value = "[1,2,3]";

        // Act
        var result = value.IsValidJson();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidJson_ValidObject_ReturnsTrue()
    {
        // Arrange
        var value = "{\"name\":\"John\"}";

        // Act
        var result = value.IsValidJson();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidJson_ValidString_ReturnsTrue()
    {
        // Arrange
        var value = "\"hello\"";

        // Act
        var result = value.IsValidJson();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidJson_WhitespaceOnly_ReturnsFalse()
    {
        // Arrange
        var value = "   ";

        // Act
        var result = value.IsValidJson();

        // Assert
        result.Should().BeFalse();
    }

    #endregion IsValidJson Tests

    #region ToXmlContent Tests

    [Fact]
    public void ToXmlContent_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToXmlContent();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToXmlContent_SimpleValue_ReturnsUnchanged()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.ToXmlContent();

        // Assert
        result.Should().Be("hello");
    }

    [Fact]
    public void ToXmlContent_ValueWithAmpersand_EscapesCorrectly()
    {
        // Arrange
        var value = "Tom & Jerry";

        // Act
        var result = value.ToXmlContent();

        // Assert
        result.Should().Be("Tom &amp; Jerry");
    }

    [Fact]
    public void ToXmlContent_ValueWithApostrophe_EscapesCorrectly()
    {
        // Arrange
        var value = "it's fine";

        // Act
        var result = value.ToXmlContent();

        // Assert
        result.Should().Be("it&apos;s fine");
    }

    [Fact]
    public void ToXmlContent_ValueWithLessThan_EscapesCorrectly()
    {
        // Arrange
        var value = "<tag>";

        // Act
        var result = value.ToXmlContent();

        // Assert
        result.Should().Be("&lt;tag&gt;");
    }

    [Fact]
    public void ToXmlContent_ValueWithQuotes_EscapesCorrectly()
    {
        // Arrange
        var value = "say \"hello\"";

        // Act
        var result = value.ToXmlContent();

        // Assert
        result.Should().Be("say &quot;hello&quot;");
    }

    #endregion ToXmlContent Tests

    #region FromXmlContent Tests

    [Fact]
    public void FromXmlContent_EscapedAmpersand_UnescapesCorrectly()
    {
        // Arrange
        var value = "Tom &amp; Jerry";

        // Act
        var result = value.FromXmlContent();

        // Assert
        result.Should().Be("Tom & Jerry");
    }

    [Fact]
    public void FromXmlContent_EscapedLessThan_UnescapesCorrectly()
    {
        // Arrange
        var value = "&lt;tag&gt;";

        // Act
        var result = value.FromXmlContent();

        // Assert
        result.Should().Be("<tag>");
    }

    [Fact]
    public void FromXmlContent_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.FromXmlContent();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FromXmlContent_RoundTrip_PreservesOriginal()
    {
        // Arrange
        var original = "<tag>Tom & Jerry say \"hello\"</tag>";

        // Act
        var escaped = original.ToXmlContent();
        var unescaped = escaped.FromXmlContent();

        // Assert
        unescaped.Should().Be(original);
    }

    [Fact]
    public void FromXmlContent_SimpleValue_ReturnsUnchanged()
    {
        // Arrange
        var value = "hello";

        // Act
        var result = value.FromXmlContent();

        // Assert
        result.Should().Be("hello");
    }

    #endregion FromXmlContent Tests

    #region Integration Tests

    [Fact]
    public void CsvRoundTrip_ComplexData_PreservesOriginal()
    {
        // Arrange
        var values = new[] { "name", "age, years", "quote: \"hi\"", "city" };

        // Act
        var csvLine = values.ToCsvLine();
        var parsed = csvLine.SplitCsvLine();

        // Assert
        parsed.Should().Equal(values);
    }

    [Fact]
    public void JsonRoundTrip_ComplexData_PreservesOriginal()
    {
        // Arrange
        var original = "Line1\nLine2\tTab\"Quote\"\\Backslash";

        // Act
        var escaped = original.ToJsonString();
        var unescaped = escaped.FromJsonString();

        // Assert
        unescaped.Should().Be(original);
    }

    [Fact]
    public void XmlRoundTrip_ComplexData_PreservesOriginal()
    {
        // Arrange
        var original = "<tag attr=\"value\">Tom & Jerry's show</tag>";

        // Act
        var escaped = original.ToXmlContent();
        var unescaped = escaped.FromXmlContent();

        // Assert
        unescaped.Should().Be(original);
    }

    #endregion Integration Tests
}