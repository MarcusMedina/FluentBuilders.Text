# FluentBuilders.Text

**Enterprise-grade** multi-platform string extension library with semantic namespaces.

[![NuGet](https://img.shields.io/badge/nuget-v1.0.0-blue)](https://www.nuget.org/packages/MarcusMedina.Fluent.Text)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)]()
[![Build](https://img.shields.io/badge/build-passing-brightgreen)]()
[![Coverage](https://img.shields.io/badge/coverage-95%25-brightgreen)]()
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

---

## 🎯 Philosophy

**Semantic Namespaces - Zero LINQ Pollution!**

Unlike LINQ which adds 200+ methods to every collection, FluentBuilders.Text uses **semantic namespaces**:
- Need casing? Import `MarcusMedina.Fluent.Text.Casing`
- Need extraction? Import `MarcusMedina.Fluent.Text.Extraction`
- Need everything? Import multiple namespaces

**Result:** Clean IntelliSense, intentional API surface, better developer experience.

---

## 🚀 Platforms

### ✅ Production-Ready

- **[C# / .NET 8.0](./csharp/)** - `MarcusMedina.Fluent.Text` on NuGet
  - 95+ extension methods across 7 semantic namespaces
  - 95%+ test coverage, zero vulnerabilities
  - Enterprise-grade CI/CD with multi-platform testing
  - *Note: Validation methods excluded - C# has built-in `string.IsNullOrEmpty()` and `string.IsNullOrWhiteSpace()`*

### 🚧 Planned

- **[Node.js / TypeScript](./node/)** - `@marcusmedina/fluent-text` on npm
- **[Python](./python/)** - `mm-fluent-text` on PyPI

---

## 📖 Quick Start

### C# / .NET

**Installation:**
```bash
dotnet add package MarcusMedina.Fluent.Text
```

**Usage:**
```csharp
// Import only what you need
using MarcusMedina.Fluent.Text.Casing;
using MarcusMedina.Fluent.Text.Validation;
using MarcusMedina.Fluent.Text.Extraction;

// Clean, focused IntelliSense
"hello world".ToPascalCase();           // "HelloWorld"
"john doe".ToNameCase();                // "John Doe"
"".IsNullOrWhiteSpace();                // true
"Email: user@test.com".ExtractEmails(); // ["user@test.com"]
```

**Available Namespaces:**
- `Casing` - 13 case transformations (PascalCase, camelCase, kebab-case, snake_case, etc.)
- `Pattern` - 6 pattern matching methods (IsLike, In, Between, etc.)
- `Extraction` - 17+ data extraction methods (emails, URLs, hashtags, etc.)
- `Counting` - 10 counting operations (words, sentences, vowels, etc.)
- `Manipulation` - 11 manipulation methods (Reverse, Shuffle, Mask, Truncate, etc.)
- `DataFormat` - 35+ encoding methods (CSV, JSON, Base64, URL, HTML, XML, Hex)
- `LineEndings` - 4 line ending conversions (Unix, Windows, Mac, Normalize)

📚 **[Full C# Documentation →](./csharp/README.md)**

---

## 📖 Behavioral Specifications

**Language-agnostic specifications** for all implementations:

📋 [**SPECIFICATION.md**](SPECIFICATION.md) - Complete behavioral specification index

Individual category specifications:
- [CASING_RULES.md](CASING_RULES.md) - 13 case transformations
- [VALIDATION_RULES.md](VALIDATION_RULES.md) - 2 validation methods (IsEmpty, IsWhiteSpace - use built-in `string.IsNullOrEmpty()` and `string.IsNullOrWhiteSpace()` for others)
- [PATTERN_RULES.md](PATTERN_RULES.md) - 6 pattern matching methods
- [EXTRACTION_RULES.md](EXTRACTION_RULES.md) - 17+ data extraction methods
- [COUNTING_RULES.md](COUNTING_RULES.md) - 10 counting operations
- [MANIPULATION_RULES.md](MANIPULATION_RULES.md) - 11 manipulation methods
- [LINEENDING_RULES.md](LINEENDING_RULES.md) - 4 line ending conversions
- [DATAFORMAT_RULES.md](DATAFORMAT_RULES.md) - 35+ encoding methods

These specifications ensure **consistent behavior** across C#, Node.js, Python, and future implementations.

---

## 🏆 Quality Standards

**Enterprise-Grade Quality:**
- ✅ **95%+ Test Coverage** - xUnit + FluentAssertions
- ✅ **Zero Vulnerabilities** - Continuous security scanning
- ✅ **Multi-Platform CI/CD** - Ubuntu, Windows, macOS
- ✅ **Complete XML Docs** - IntelliSense-friendly
- ✅ **Null-Safety First** - `ArgumentNullException.ThrowIfNull`
- ✅ **Culture-Aware** - `CultureInfo.InvariantCulture`
- ✅ **TDD-Developed** - Red-Green-Refactor methodology

---

## 📦 Version History

See [CHANGELOG.md](CHANGELOG.md) for detailed release notes.

**Latest Release:** [1.0.0](CHANGELOG.md#100---2025-10-04) (2025-10-04)
- Initial enterprise-grade release
- 95+ extension methods across 7 namespaces (C#)
- Complete test coverage and documentation
- Production-ready CI/CD pipeline

---

## 🤝 Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

Contributions are welcome! This project follows:
- **TDD** - Red-Green-Refactor cycle
- **Vertical Slices** - Complete features (docs → tests → code → README)
- **Conventional Commits** - Structured commit history
- **Quality Gates** - 95%+ coverage, zero warnings, security scans

---

## 📄 License

MIT © Marcus Ackre Medina - See [LICENSE](LICENSE)

---

## 👤 Author

**Marcus Ackre Medina**
- 🌐 Website: [marcusmedina.pro](https://marcusmedina.pro)
- 💻 GitHub: [@MarcusMedina](https://github.com/MarcusMedina)
- 📦 NuGet: [MarcusMedina.Fluent.Text](https://www.nuget.org/packages/MarcusMedina.Fluent.Text)

> _Building educational tools and enterprise-grade libraries. Much of what I code is for my students – but I hope others find it useful too._

---

## ⭐ Show Your Support

If you find this library useful:
- ⭐ Star this repository
- 📦 Use it in your projects
- 🐛 Report issues
- 💡 Suggest features
- 🤝 Contribute code

---

**Built with ❤️ using Test-Driven Development and Vertical Slice Architecture**
