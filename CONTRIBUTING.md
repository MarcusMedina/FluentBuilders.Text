# Contributing to FluentBuilders.Text

Thank you for your interest in contributing! This project follows **enterprise-grade quality standards** and uses **TDD + Vertical Slice Architecture**.

---

## 🚀 Development Setup

### C# / .NET

**Prerequisites:**
- .NET 8.0 SDK or higher
- Git
- IDE with EditorConfig support (VS Code, Visual Studio, Rider)

**Setup Steps:**
```bash
# 1. Clone and navigate
git clone https://github.com/MarcusMedinaPro/FluentBuilders.Text.git
cd FluentBuilders.Text/csharp

# 2. Restore dependencies
dotnet restore

# 3. Build with zero warnings
dotnet build /p:TreatWarningsAsErrors=true

# 4. Run tests with coverage
dotnet test /p:CollectCoverage=true /p:Threshold=95

# 5. Verify code format
dotnet format --verify-no-changes
```

### Node.js / TypeScript (planned)
```bash
cd node
npm install
npm test
```

### Python (planned)
```bash
cd python
pip install -e .[dev]
pytest
```

---

## 📋 Development Guidelines

### TDD Workflow (MANDATORY)
All features **MUST** follow the Red-Green-Refactor cycle:

**1️⃣ RED Phase** - Write failing tests first
```csharp
[Fact]
public void ToUpperCase_WithLowercaseString_ShouldReturnUppercase()
{
    "hello".ToUpperCase().Should().Be("HELLO");
}
```
Run: `dotnet test` → ❌ FAILS

**2️⃣ GREEN Phase** - Implement minimal code
```csharp
public static string ToUpperCase(this string value)
{
    ArgumentNullException.ThrowIfNull(value);
    return value.ToUpper(CultureInfo.InvariantCulture);
}
```
Run: `dotnet test` → ✅ PASSES

**3️⃣ REFACTOR Phase** - Improve code quality
- Add complete XML documentation
- Optimize performance
- Update README with examples
- Verify: `dotnet test` → ✅ STILL PASSES

### Vertical Slice Architecture
Each feature must be a **complete vertical slice**:

1. **XML Documentation** - `<summary>`, `<param>`, `<returns>`, `<exception>`, `<example>`
2. **Unit Tests** - Happy path, edge cases, null handling, error scenarios
3. **Implementation** - Null-safe, culture-aware, performance-optimized
4. **README Update** - User-facing examples and usage
5. **CHANGELOG Entry** - Document the new feature
6. **Git Commit** - Single focused commit with conventional format

### Code Quality Standards

**Must Pass:**
- ✅ **95%+ Line Coverage** - Use FluentAssertions for readable tests
- ✅ **Zero Warnings** - Build with `/p:TreatWarningsAsErrors=true`
- ✅ **Zero Vulnerabilities** - Run `dotnet list package --vulnerable`
- ✅ **Format Compliance** - Run `dotnet format --verify-no-changes`
- ✅ **Null Safety** - Always use `ArgumentNullException.ThrowIfNull(value)`
- ✅ **Culture Awareness** - Use `CultureInfo.InvariantCulture` for string operations

**EditorConfig Enforcement:**
- File-scoped namespaces (C# 10+)
- Private fields with `_` prefix
- Interfaces with `I` prefix
- Async methods with `Async` suffix

---

## 🎯 Contribution Process

### 1. Pick or Create an Issue
- Check existing issues for features/bugs
- Create new issue for discussion before major changes
- Get approval from maintainers for architectural changes

### 2. Create Feature Branch
```bash
git checkout -b feature/add-ispalindrome-method
```

**Branch Naming:**
- `feature/` - New features
- `fix/` - Bug fixes
- `docs/` - Documentation only
- `refactor/` - Code refactoring
- `test/` - Test improvements

### 3. Implement Using TDD
```bash
# RED: Write failing test
dotnet test  # Should fail

# GREEN: Implement method
dotnet test  # Should pass

# REFACTOR: Complete docs, optimize
dotnet test  # Still passes
```

### 4. Update Documentation
- Add XML docs with `<example>` tags
- Update `csharp/README.md` with usage examples
- Add entry to `CHANGELOG.md` under `[Unreleased]`

### 5. Run Quality Checks
```bash
# Run all tests with coverage
dotnet test /p:CollectCoverage=true /p:Threshold=95

# Security scan
dotnet list package --vulnerable --include-transitive

# Format check
dotnet format --verify-no-changes

# Build with warnings as errors
dotnet build /p:TreatWarningsAsErrors=true
```

### 6. Commit with Conventional Format
```bash
git add .
git commit -m "feat: Add IsPalindrome extension method

- Add IsPalindrome() to StringValidationExtensions
- Include comprehensive tests for edge cases
- Update README with examples
- 100% test coverage maintained"
```

**Commit Prefixes:**
- `feat:` - New feature
- `fix:` - Bug fix
- `docs:` - Documentation only
- `test:` - Test improvements
- `refactor:` - Code refactoring
- `perf:` - Performance improvements
- `ci:` - CI/CD changes
- `chore:` - Maintenance tasks

### 7. Push and Create Pull Request
```bash
git push origin feature/add-ispalindrome-method
```

**PR Template:**
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] New feature (feat)
- [ ] Bug fix (fix)
- [ ] Documentation (docs)
- [ ] Refactoring (refactor)

## Checklist
- [ ] Tests written (TDD: Red-Green-Refactor)
- [ ] 95%+ coverage maintained
- [ ] XML documentation complete
- [ ] README.md updated
- [ ] CHANGELOG.md updated
- [ ] EditorConfig compliance (`dotnet format`)
- [ ] Zero warnings (`/p:TreatWarningsAsErrors=true`)
- [ ] Security scan passed
- [ ] All CI checks passing
```

---

## 🧪 Testing Requirements

### Test Categories
Every extension method needs:

1. **Happy Path** - Expected usage
2. **Edge Cases** - Empty strings, single char, very long strings
3. **Null Handling** - Should throw `ArgumentNullException`
4. **Unicode** - Non-ASCII characters
5. **Whitespace** - Spaces, tabs, newlines
6. **Special Characters** - Numbers, symbols, punctuation

### Example Test Suite
```csharp
public class StringCasingExtensionsTests
{
    [Fact]
    public void ToUpperCase_WithLowercaseString_ShouldReturnUppercase()
    {
        "hello".ToUpperCase().Should().Be("HELLO");
    }

    [Fact]
    public void ToUpperCase_WithEmptyString_ShouldReturnEmpty()
    {
        "".ToUpperCase().Should().BeEmpty();
    }

    [Fact]
    public void ToUpperCase_WithNull_ShouldThrowArgumentNullException()
    {
        string? value = null;
        Action act = () => value!.ToUpperCase();
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToUpperCase_WithUnicode_ShouldHandleCorrectly()
    {
        "café".ToUpperCase().Should().Be("CAFÉ");
    }
}
```

---

## 📚 Documentation Requirements

### XML Documentation Template
```csharp
/// <summary>
/// Brief description of what the method does.
/// </summary>
/// <param name="value">Description of parameter.</param>
/// <returns>Description of return value.</returns>
/// <exception cref="ArgumentNullException">
/// Thrown when <paramref name="value"/> is null.
/// </exception>
/// <example>
/// <code>
/// "hello".ToUpperCase()  // "HELLO"
/// "world".ToUpperCase()  // "WORLD"
/// </code>
/// </example>
```

---

## 🚫 What NOT to Do

❌ **Don't skip tests** - TDD is mandatory
❌ **Don't commit warnings** - Zero warnings policy
❌ **Don't skip documentation** - XML docs required
❌ **Don't ignore EditorConfig** - Code style must match
❌ **Don't batch multiple features** - One slice per PR
❌ **Don't use culture-dependent operations** - Use `InvariantCulture`
❌ **Don't skip null checks** - Always validate inputs
❌ **Don't commit without CHANGELOG entry** - Document all changes

---

## 🎓 Learning Resources

**New to TDD?**
- [Test-Driven Development by Example](https://www.amazon.com/Test-Driven-Development-Kent-Beck/dp/0321146530) - Kent Beck
- [Growing Object-Oriented Software](https://www.amazon.com/Growing-Object-Oriented-Software-Guided-Tests/dp/0321503627) - Freeman & Pryce

**New to Vertical Slices?**
- [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/) - Jimmy Bogard
- [Feature Slices for ASP.NET Core MVC](https://docs.microsoft.com/en-us/archive/msdn-magazine/2016/september/asp-net-core-feature-slices-for-asp-net-core-mvc)

**C# Best Practices:**
- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Framework Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/)

---

## 📞 Getting Help

- 💬 **Questions:** Open a [GitHub Discussion](https://github.com/MarcusMedinaPro/FluentBuilders.Text/discussions)
- 🐛 **Bugs:** Open a [GitHub Issue](https://github.com/MarcusMedinaPro/FluentBuilders.Text/issues)
- 💡 **Feature Ideas:** Start a discussion first

---

## 🤝 Code of Conduct

**Be Kind. Be Respectful. Be Constructive.**

We're all here to:
- Learn together
- Build quality software
- Share knowledge
- Help each other grow

Harassment, discrimination, or toxic behavior will not be tolerated.

---

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

**Thank you for making FluentBuilders.Text better! 🎉**
