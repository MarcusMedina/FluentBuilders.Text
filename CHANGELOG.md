# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Planned Features
- Node.js / TypeScript implementation
- Python implementation
- Performance benchmarks project
- Additional data format support (YAML, TOML)

---

## [1.1.0] - 2025-10-08

### Changed
- **Package Tags**: Added "strings" and "extension" tags for better NuGet discoverability
- **Documentation**: Removed ToUpperCase/ToLowerCase from README (use built-in .ToUpper()/.ToLower() instead)
- **Metadata**: Updated package metadata for improved searchability

### Infrastructure
- Configured automated NuGet publishing with GitHub Actions
- Streamlined release workflow with API key authentication

---

## [1.0.0] - 2025-10-04

### 🎉 Initial Release - Enterprise-Grade String Extensions

This is the first production-ready release of FluentBuilders.Text for C# / .NET 8.0.

### 🏗️ Core Architecture

**Semantic Namespaces (Anti-LINQ Pollution)**
- `MarcusMedina.Fluent.Text.Casing` - Case transformation methods
- `MarcusMedina.Fluent.Text.Pattern` - Pattern matching methods
- `MarcusMedina.Fluent.Text.Extraction` - Data extraction methods
- `MarcusMedina.Fluent.Text.Counting` - Counting operations
- `MarcusMedina.Fluent.Text.Manipulation` - String manipulation
- `MarcusMedina.Fluent.Text.DataFormat` - Encoding and data formats
- `MarcusMedina.Fluent.Text.LineEndings` - Line ending conversions

**Note:** Validation methods (IsEmpty, IsNullOrEmpty, IsNullOrWhiteSpace) are not included in C# since `string.IsNullOrEmpty()` and `string.IsNullOrWhiteSpace()` are already built-in.

### ✨ Features

#### 🔹 Casing (13 methods)
- `ToUpperCase()`, `ToLowerCase()` - Basic case conversion
- `ToPascalCase()` - Convert to PascalCase
- `ToCamelCase()` - Convert to camelCase
- `ToKebabCase()` - Convert to kebab-case
- `ToSnakeCase()` - Convert to snake_case
- `ToScreamingSnakeCase()` - Convert to SCREAMING_SNAKE_CASE
- `ToTitleCase()` - Convert to Title Case
- `ToSentenceCase()` - Convert to Sentence case
- `ToNameCase()` - Smart name casing (handles O'Brien, McDonald, von Neumann, etc.)
- `ToRandomCase()` - Random case per character
- `ToAlternatingCase()` - Alternating upper/lower case
- `ToLeetSpeak()` - Convert to l33t speak

#### 🔹 Line Endings (4 methods)
- `ToUnixLineEndings()` - Convert to LF (`\n`)
- `ToWindowsLineEndings()` - Convert to CRLF (`\r\n`)
- `ToMacLineEndings()` - Convert to CR (`\r`)
- `NormalizeLineEndings()` - Normalize to specified format

#### 🔹 Pattern Matching (6 methods)
- `IsLike()` - SQL-style LIKE pattern matching (%, _)
- `ContainsText()`, `StartsWithText()`, `EndsWithText()` - Case-insensitive checks
- `In()` - Check if string is in collection
- `Between()` - Check if string is alphabetically between two values

#### 🔹 Extraction (17+ methods)
**Basic Extraction:**
- `Left()`, `Right()`, `Mid()` - Substring extraction

**Data Extraction:**
- `ExtractEmails()` - Extract email addresses
- `ExtractUrls()` - Extract URLs
- `ExtractPhoneNumbers()` - Extract phone numbers
- `ExtractDates()` - Extract date strings
- `ExtractNumbers()` - Extract numeric values
- `ExtractHashtags()` - Extract #hashtags
- `ExtractMentions()` - Extract @mentions
- `ExtractBetween()` - Extract text between delimiters
- `ExtractWordsContaining()`, `ExtractWordsStartingWith()`, `ExtractWordsEndingWith()`
- `ExtractWordsOfLength()` - Extract words by length
- `ExtractAllWords()`, `ExtractAllSentences()` - Extract text structures

#### 🔹 Counting (10 methods)
- `CountWords()`, `CountSentences()` - Text structure counting
- `CountVowels()`, `CountConsonants()` - Letter type counting
- `CountOccurrences()` - Count char/string occurrences
- `CountLines()` - Count line breaks
- `CountDigits()`, `CountLetters()` - Character type counting
- `CountUppercase()`, `CountLowercase()` - Case counting

#### 🔹 Manipulation (11 methods)
- `Reverse()` - Reverse string
- `Shuffle()`, `ShuffleWords()`, `ShuffleSentences()` - Randomize order
- `Repeat()` - Repeat string n times
- `Truncate()` - Truncate with ellipsis
- `Mask()` - Mask sensitive data (credit cards, passwords)
- `RemoveWhitespace()`, `CollapseWhitespace()` - Whitespace cleanup
- `InsertAt()` - Insert text at position
- `WrapTextAt()` - Text wrapping with options

#### 🔹 Data Formats & Encoding (35+ methods)
**CSV Operations:**
- `ToCsvField()`, `FromCsvField()` - CSV field escaping
- `SplitCsvLine()`, `ToCsvLine()` - CSV line parsing/formatting
- `ToCsv()` (3 overloads) - Array/list to CSV
- `FromCsvToArray()`, `FromCsvToList()` - CSV parsing

**JSON Operations:**
- `ToJsonString()`, `FromJsonString()` - JSON string escaping
- `IsValidJson()` - JSON validation
- `ToJsonArray()` (3 overloads) - Array/list to JSON
- `FromJsonToArray()`, `FromJsonToList()` - JSON parsing

**Encoding:**
- **Base64**: `ToBase64()`, `FromBase64()`, `IsValidBase64()`
- **URL**: `ToUrlEncoded()`, `FromUrlEncoded()`
- **HTML**: `ToHtmlEncoded()`, `FromHtmlEncoded()`
- **XML**: `ToXmlContent()`, `FromXmlContent()`
- **Hex**: `ToHex()`, `FromHex()`, `IsValidHex()`

### 🏆 Quality Standards

**Testing:**
- ✅ 95%+ line coverage with xUnit
- ✅ FluentAssertions for readable test assertions
- ✅ Coverlet for code coverage analysis
- ✅ Comprehensive test suite (400+ tests)

**Security:**
- ✅ Zero known vulnerabilities
- ✅ Microsoft.CodeAnalysis.NetAnalyzers enabled
- ✅ Null-safety with `ArgumentNullException.ThrowIfNull`
- ✅ TreatWarningsAsErrors enabled

**Documentation:**
- ✅ Complete XML documentation for all public APIs
- ✅ Code examples in XML docs
- ✅ Comprehensive README with category-based examples
- ✅ IntelliSense-friendly documentation

**Code Quality:**
- ✅ .editorconfig for consistent code style
- ✅ File-scoped namespaces (C# 10+)
- ✅ Nullable reference types enabled
- ✅ Culture-aware string operations (`CultureInfo.InvariantCulture`)

**CI/CD:**
- ✅ Enterprise-grade GitHub Actions workflows
- ✅ Multi-platform testing (Ubuntu, Windows, macOS)
- ✅ Automated NuGet publishing on version tags
- ✅ Dependabot for automated dependency updates
- ✅ Code coverage reporting with Codecov

### 📦 NuGet Package

**Package ID:** `MarcusMedina.Fluent.Text`
**Version:** 1.0.0
**Target Framework:** .NET 8.0
**License:** MIT
**Author:** Marcus Ackre Medina

### 🛠️ Development Methodology

- ✅ **Test-Driven Development (TDD)** - Red-Green-Refactor cycle
- ✅ **Vertical Slice Architecture** - Complete features (docs → tests → code → README)
- ✅ **Conventional Commits** - Structured commit history
- ✅ **Semantic Versioning** - Predictable version scheme

---

## Version History

- **[1.0.0] - 2025-10-04**: Initial enterprise-grade release
