namespace MarcusMedina.Fluent.Text.Tests.Extensions;
#pragma warning disable IDE0058 // Expression value is never used

using MarcusMedina.Fluent.Text.Extensions.LineEndings;
using FluentAssertions;
using Xunit;

public class StringLineEndingExtensionsTests
{
    #region ToUnixLineEndings Tests

    [Fact]
    public void ToUnixLineEndings_MacLineEndings_ConvertsToUnix()
    {
        // Arrange
        var value = "hello\rworld";

        // Act
        var result = value.ToUnixLineEndings();

        // Assert
        result.Should().Be("hello\nworld");
    }

    [Fact]
    public void ToUnixLineEndings_MixedLineEndings_ConvertsToUnix()
    {
        // Arrange
        var value = "hello\r\nworld\ntest\rend";

        // Act
        var result = value.ToUnixLineEndings();

        // Assert
        result.Should().Be("hello\nworld\ntest\nend");
    }

    [Fact]
    public void ToUnixLineEndings_NoLineEndings_RemainsUnchanged()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToUnixLineEndings();

        // Assert
        result.Should().Be("hello world");
    }

    [Fact]
    public void ToUnixLineEndings_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToUnixLineEndings();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToUnixLineEndings_UnixLineEndings_RemainsUnchanged()
    {
        // Arrange
        var value = "hello\nworld";

        // Act
        var result = value.ToUnixLineEndings();

        // Assert
        result.Should().Be("hello\nworld");
    }

    [Fact]
    public void ToUnixLineEndings_WindowsLineEndings_ConvertsToUnix()
    {
        // Arrange
        var value = "hello\r\nworld";

        // Act
        var result = value.ToUnixLineEndings();

        // Assert
        result.Should().Be("hello\nworld");
    }

    #endregion ToUnixLineEndings Tests

    #region ToWindowsLineEndings Tests

    [Fact]
    public void ToWindowsLineEndings_MacLineEndings_ConvertsToWindows()
    {
        // Arrange
        var value = "hello\rworld";

        // Act
        var result = value.ToWindowsLineEndings();

        // Assert
        result.Should().Be("hello\r\nworld");
    }

    [Fact]
    public void ToWindowsLineEndings_MixedLineEndings_ConvertsToWindows()
    {
        // Arrange
        var value = "hello\r\nworld\ntest\rend";

        // Act
        var result = value.ToWindowsLineEndings();

        // Assert
        result.Should().Be("hello\r\nworld\r\ntest\r\nend");
    }

    [Fact]
    public void ToWindowsLineEndings_NoLineEndings_RemainsUnchanged()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToWindowsLineEndings();

        // Assert
        result.Should().Be("hello world");
    }

    [Fact]
    public void ToWindowsLineEndings_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToWindowsLineEndings();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToWindowsLineEndings_UnixLineEndings_ConvertsToWindows()
    {
        // Arrange
        var value = "hello\nworld";

        // Act
        var result = value.ToWindowsLineEndings();

        // Assert
        result.Should().Be("hello\r\nworld");
    }

    [Fact]
    public void ToWindowsLineEndings_WindowsLineEndings_RemainsUnchanged()
    {
        // Arrange
        var value = "hello\r\nworld";

        // Act
        var result = value.ToWindowsLineEndings();

        // Assert
        result.Should().Be("hello\r\nworld");
    }

    #endregion ToWindowsLineEndings Tests

    #region ToMacLineEndings Tests

    [Fact]
    public void ToMacLineEndings_MacLineEndings_RemainsUnchanged()
    {
        // Arrange
        var value = "hello\rworld";

        // Act
        var result = value.ToMacLineEndings();

        // Assert
        result.Should().Be("hello\rworld");
    }

    [Fact]
    public void ToMacLineEndings_MixedLineEndings_ConvertsToMac()
    {
        // Arrange
        var value = "hello\r\nworld\ntest";

        // Act
        var result = value.ToMacLineEndings();

        // Assert
        result.Should().Be("hello\rworld\rtest");
    }

    [Fact]
    public void ToMacLineEndings_NoLineEndings_RemainsUnchanged()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.ToMacLineEndings();

        // Assert
        result.Should().Be("hello world");
    }

    [Fact]
    public void ToMacLineEndings_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.ToMacLineEndings();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToMacLineEndings_UnixLineEndings_ConvertsToMac()
    {
        // Arrange
        var value = "hello\nworld";

        // Act
        var result = value.ToMacLineEndings();

        // Assert
        result.Should().Be("hello\rworld");
    }

    [Fact]
    public void ToMacLineEndings_WindowsLineEndings_ConvertsToMac()
    {
        // Arrange
        var value = "hello\r\nworld";

        // Act
        var result = value.ToMacLineEndings();

        // Assert
        result.Should().Be("hello\rworld");
    }

    #endregion ToMacLineEndings Tests

    #region NormalizeLineEndings Tests

    [Fact]
    public void NormalizeLineEndings_MixedLineEndings_NormalizesToSystemDefault()
    {
        // Arrange
        var value = "hello\r\nworld\ntest\rend";

        // Act
        var result = value.NormalizeLineEndings();

        // Assert
        result.Should().Contain("hello").And.Contain("world").And.Contain("test").And.Contain("end");
        // Verify consistent line endings (all should be Environment.NewLine)
        var lines = result.Split(Environment.NewLine);
        lines.Should().HaveCount(4);
    }

    [Fact]
    public void NormalizeLineEndings_NoLineEndings_RemainsUnchanged()
    {
        // Arrange
        var value = "hello world";

        // Act
        var result = value.NormalizeLineEndings();

        // Assert
        result.Should().Be("hello world");
    }

    [Fact]
    public void NormalizeLineEndings_NullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var act = () => value!.NormalizeLineEndings();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void NormalizeLineEndings_UnixLineEndings_NormalizesToSystemDefault()
    {
        // Arrange
        var value = "hello\nworld\ntest";

        // Act
        var result = value.NormalizeLineEndings();

        // Assert
        var lines = result.Split(Environment.NewLine);
        lines.Should().HaveCount(3);
        lines[0].Should().Be("hello");
        lines[1].Should().Be("world");
        lines[2].Should().Be("test");
    }

    [Fact]
    public void NormalizeLineEndings_WindowsLineEndings_NormalizesToSystemDefault()
    {
        // Arrange
        var value = "hello\r\nworld\r\ntest";

        // Act
        var result = value.NormalizeLineEndings();

        // Assert
        var lines = result.Split(Environment.NewLine);
        lines.Should().HaveCount(3);
        lines[0].Should().Be("hello");
        lines[1].Should().Be("world");
        lines[2].Should().Be("test");
    }

    #endregion NormalizeLineEndings Tests
}