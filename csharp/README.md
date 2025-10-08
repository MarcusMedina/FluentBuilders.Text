# FluentBuilders.Text (C# / .NET 8.0)

**Enterprise-grade** fluent string extensions for C# with comprehensive testing and documentation.

[![NuGet](https://img.shields.io/badge/nuget-v1.0.0-blue)]()
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)]()
[![Build](https://img.shields.io/badge/build-passing-brightgreen)]()
[![Coverage](https://img.shields.io/badge/coverage-95%25-brightgreen)]()
[![License](https://img.shields.io/badge/license-MIT-green)]()

---

## 🎯 Philosophy

**Semantic Namespaces - Zero LINQ Pollution!**

Each extension category has **its own namespace**. Import only what you need, keeping IntelliSense clean and focused.

Unlike LINQ which adds 200+ methods to every collection, FluentBuilders.Text lets you choose:
- Need casing? Import `MarcusMedina.Fluent.Text.Extensions.Casing`
- Need extraction? Import `MarcusMedina.Fluent.Text.Extensions.Extraction`
- Need everything? Import multiple namespaces

**Result:** Clean IntelliSense, intentional API surface, better developer experience.

---

## 🚀 Installation

```bash
dotnet add package MarcusMedina.Fluent.Text
```

---

## 📖 Quick Start

```csharp
// Import only what you need
using MarcusMedina.Fluent.Text.Extensions.Casing;
using MarcusMedina.Fluent.Text.Extensions.Extraction;

"hello world".ToPascalCase();           // "HelloWorld"
"hello world".Left(5);                  // "hello"
"Email: user@test.com".ExtractEmails(); // ["user@test.com"]

// C# built-ins (no extensions needed):
string.IsNullOrEmpty(value);
string.IsNullOrWhiteSpace(value);
```

---

## 🏆 Quality Standards

This library follows **enterprise-grade quality standards**:

- ✅ **95%+ Test Coverage** - Comprehensive unit tests with xUnit + FluentAssertions
- ✅ **Zero Vulnerabilities** - Continuous security scanning with Dependabot
- ✅ **Multi-Platform CI/CD** - Tested on Ubuntu, Windows, and macOS
- ✅ **Complete XML Documentation** - Every method documented with examples
- ✅ **Null-Safety First** - `ArgumentNullException.ThrowIfNull` everywhere
- ✅ **Culture-Aware** - `CultureInfo.InvariantCulture` for predictable results
- ✅ **TDD-Developed** - Red-Green-Refactor for every feature
- ✅ **Performance Optimized** - Benchmarked for production use

---

## 📚 Extension Categories

### 🔹 Casing
**Namespace:** `MarcusMedina.Fluent.Text.Extensions.Casing`

```csharp
"hello world".ToPascalCase()          // "HelloWorld"
"HelloWorld".ToCamelCase()            // "helloWorld"
"HelloWorld".ToKebabCase()            // "hello-world"
"HelloWorld".ToSnakeCase()            // "hello_world"
"hello world".ToScreamingSnakeCase()  // "HELLO_WORLD"
"hello world".ToTitleCase()           // "Hello World"
"hello world".ToProperCase()          // "Hello World" (alias for TitleCase)
"hello world".ToSentenceCase()        // "Hello world"
"john doe".ToNameCase()               // "John Doe"
```

**Methods:** ToPascalCase, ToCamelCase, ToKebabCase, ToSnakeCase, ToScreamingSnakeCase, ToTitleCase, ToProperCase, ToSentenceCase, ToNameCase (9 core methods + additional variants like ToLeetSpeak, ToAlternatingCase, ToRandomCase)

---

### 🔹 Line Endings
**Namespace:** `MarcusMedina.Fluent.Text.Extensions.LineEndings`

```csharp
"text\r\nfile".ToUnixLineEndings()     // "text\nfile"
"text\nfile".ToWindowsLineEndings()    // "text\r\nfile"
```

**Methods:** ToUnixLineEndings, ToWindowsLineEndings, ToMacLineEndings, NormalizeLineEndings

---

### 🔹 Pattern Matching
**Namespace:** `MarcusMedina.Fluent.Text.Extensions.Pattern`

```csharp
"test".IsLike("te%")                      // true (SQL LIKE)
"hello".In(["hello", "world"])            // true
"bob".Between("alice", "charlie")         // true
```

**Methods:** IsLike, ContainsText, StartsWithText, EndsWithText, In, Between

---

### 🔹 Extraction
**Namespace:** `MarcusMedina.Fluent.Text.Extensions.Extraction`

```csharp
// Basic extraction
"hello world".Left(5)           // "hello"
"hello world".Right(5)          // "world"
"hello world".Mid(6, 5)         // "world"

// Data extraction
"Contact: user@test.com, admin@test.org".ExtractEmails()
// ["user@test.com", "admin@test.org"]

"Visit https://example.com".ExtractUrls()
// ["https://example.com"]

"Love #coding and #dotnet!".ExtractHashtags()
// ["#coding", "#dotnet"]

"Thanks @john and @jane!".ExtractMentions()
// ["@john", "@jane"]
```

**Methods:** Left, Right, Mid, ExtractEmails, ExtractUrls, ExtractPhoneNumbers, ExtractDates, ExtractNumbers, ExtractHashtags, ExtractMentions, ExtractBetween, ExtractWordsContaining, ExtractWordsStartingWith, ExtractWordsEndingWith, ExtractWordsOfLength, ExtractAllWords, ExtractAllSentences

---

### 🔹 Counting
**Namespace:** `MarcusMedina.Fluent.Text.Extensions.Counting`

```csharp
"Hello world, how are you?".CountWords()      // 5
"Hello world".CountVowels()                   // 3
"Hello World".CountUppercase()                // 2
"hello world".CountOccurrences('l')           // 3
```

**Methods:** CountWords, CountSentences, CountVowels, CountConsonants, CountOccurrences (char & string), CountLines, CountDigits, CountLetters, CountUppercase, CountLowercase

---

### 🔹 Manipulation
**Namespace:** `MarcusMedina.Fluent.Text.Extensions.Manipulation`

```csharp
"hello".Reverse()                             // "olleh"
"hello world today".ShuffleWords()            // "today hello world" (random)
"Ha".Repeat(3)                                // "HaHaHa"
"1234567890".Mask(4, 4)                       // "1234****90"
"This is a very long sentence".Truncate(10)   // "This is..."
"This is a very long sentence".WrapTextAt(20, breakWords: true)
// Wraps text at 20 chars
```

**Methods:** Reverse, Shuffle, ShuffleWords, ShuffleSentences, Repeat, Truncate, Mask, RemoveWhitespace, CollapseWhitespace, InsertAt, WrapTextAt

---

### 🔹 Data Formats & Encoding
**Namespace:** `MarcusMedina.Fluent.Text.Extensions.DataFormat`

```csharp
// CSV operations
"hello, world".ToCsvField()                    // "\"hello, world\""
var data = new string[,] { { "Name" }, { "John" } };
data.ToCsv();                                  // "Name\nJohn"

// JSON operations
"{\"name\":\"John\"}".IsValidJson()            // true
var list = new List<List<string>> { new() { "Name" }, new() { "John" } };
list.ToJsonArray();                            // [["Name"],["John"]]

// Encoding operations
"hello world".ToBase64()                       // "aGVsbG8gd29ybGQ="
"hello world".ToUrlEncoded()                   // "hello+world"
"<script>".ToHtmlEncoded()                     // "&lt;script&gt;"
"hello".ToHex()                                // "68656C6C6F"
"<tag>".ToXmlContent()                         // "&lt;tag&gt;"
```

**Methods:**
- **CSV**: ToCsvField, FromCsvField, SplitCsvLine, ToCsvLine, ToCsv (3 overloads), FromCsvToArray, FromCsvToList
- **JSON**: ToJsonString, FromJsonString, IsValidJson, ToJsonArray (3 overloads), FromJsonToArray, FromJsonToList
- **XML**: ToXmlContent, FromXmlContent
- **Base64**: ToBase64, FromBase64, IsValidBase64
- **URL**: ToUrlEncoded, FromUrlEncoded
- **HTML**: ToHtmlEncoded, FromHtmlEncoded
- **Hex**: ToHex, FromHex, IsValidHex

---

## ✨ Why FluentBuilders.Text?

✅ **Zero Namespace Pollution** - Semantic namespaces, import only what you need
✅ **Enterprise Quality** - 95%+ coverage, zero vulnerabilities, multi-platform CI/CD
✅ **Complete Documentation** - XML docs with examples for every method
✅ **Null-Safe & Culture-Aware** - Production-ready reliability
✅ **Educational** - Perfect for learning extension methods and TDD
✅ **Modern .NET 8.0** - File-scoped namespaces, nullable reference types

---

## 🧪 Development

### Build & Test
```bash
cd csharp

# Restore dependencies
dotnet restore

# Build with zero warnings
dotnet build /p:TreatWarningsAsErrors=true

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:Threshold=95

# Format code
dotnet format
```

### Quality Checks
```bash
# Security scan
dotnet list package --vulnerable --include-transitive

# Verify code style
dotnet format --verify-no-changes
```

### CI/CD
This project uses **enterprise-grade GitHub Actions workflows**:
- **Continuous Integration** - Quality gates, security scanning, multi-platform testing
- **Automated Releases** - Version tagging triggers NuGet publishing
- **Dependabot** - Automated dependency updates with security focus

See [.github/workflows/](../.github/workflows/) for details.

---

## 🤝 Contributing

See [../CONTRIBUTING.md](../CONTRIBUTING.md) for guidelines.

---

## 📄 License

MIT © Marcus Ackre Medina - See [../LICENSE](../LICENSE)

---

## 🚧 Status

**Experimental** - API may change before stable release
