# FluentBuilders.Text - Behavioral Specification

This document defines the **language-agnostic behavior** of all FluentBuilders.Text methods. These specifications apply to **all platform implementations** (C#, Node.js, Python, etc.).

---

## 📋 Specification Documents

Each category has a detailed behavioral specification:

### 1. [CASING_RULES.md](CASING_RULES.md)
**13 case transformation methods**
- ToUpperCase, ToLowerCase, ToPascalCase, ToCamelCase
- ToKebabCase, ToSnakeCase, ToScreamingSnakeCase
- ToTitleCase, ToSentenceCase, ToProperCase
- ToNameCase (smart name handling: O'Brien, McDonald, von Neumann)
- ToRandomCase, ToAlternatingCase, ToLeetSpeak

**Key Rules:**
- Use invariant culture for predictable results
- ToNameCase handles apostrophes, hyphens, prefixes, Roman numerals
- PascalCase/camelCase detect word boundaries (spaces, underscores, hyphens, case changes)

---

### 2. [VALIDATION_RULES.md](VALIDATION_RULES.md)
**2 validation methods** *(C# includes IsEmpty and IsWhiteSpace; use built-in `string.IsNullOrEmpty()` and `string.IsNullOrWhiteSpace()`)*

- IsEmpty - Zero length check (extension alternative to `value.Length == 0`)
- IsWhiteSpace - Non-empty whitespace only check

**Key Rules:**
- Both methods throw on null (require non-null input)
- IsEmpty returns true only for zero-length strings
- IsWhiteSpace returns true only for non-empty strings containing only whitespace
- Truth table provided comparing with C# built-ins

---

### 3. [PATTERN_RULES.md](PATTERN_RULES.md)
**6 pattern matching methods**
- IsLike - SQL-style LIKE with `%` (any chars) and `_` (single char)
- ContainsText, StartsWithText, EndsWithText - Case-insensitive checks
- In - Check if value exists in collection
- Between - Alphabetical range check (inclusive)

**Key Rules:**
- IsLike uses SQL wildcards (not regex)
- Text methods are case-insensitive by default
- Between uses string comparison (alphabetical order)

---

### 4. [EXTRACTION_RULES.md](EXTRACTION_RULES.md)
**17+ extraction methods**

**Substring:**
- Left, Right, Mid - Safe substring extraction (auto-clamping)

**Data Extraction (Regex-based):**
- ExtractEmails, ExtractUrls, ExtractPhoneNumbers
- ExtractDates, ExtractNumbers
- ExtractHashtags, ExtractMentions
- ExtractBetween (text between delimiters)

**Word Extraction:**
- ExtractWordsContaining, ExtractWordsStartingWith, ExtractWordsEndingWith
- ExtractWordsOfLength, ExtractAllWords, ExtractAllSentences

**Key Rules:**
- Substring methods clamp to string length (never throw for length overflow)
- Regex patterns should be compiled and cached
- Return empty array if no matches found

---

### 5. [COUNTING_RULES.md](COUNTING_RULES.md)
**10 counting methods**
- CountWords, CountSentences
- CountVowels, CountConsonants
- CountOccurrences (char and string overloads)
- CountLines, CountDigits, CountLetters
- CountUppercase, CountLowercase

**Key Rules:**
- Words separated by any whitespace
- Sentences end with `.`, `!`, `?`
- Vowels: a, e, i, o, u (case-insensitive)
- Consonants: all letters except vowels
- Y is a consonant

---

### 6. [MANIPULATION_RULES.md](MANIPULATION_RULES.md)
**11 manipulation methods**
- Reverse, Shuffle, ShuffleWords, ShuffleSentences
- Repeat
- Truncate (with ellipsis)
- Mask (for sensitive data)
- RemoveWhitespace, CollapseWhitespace
- InsertAt
- WrapTextAt (line wrapping)

**Key Rules:**
- Shuffle uses random (not cryptographically secure)
- Truncate includes ellipsis in maxLength
- Mask shows visibleStart + visibleEnd characters
- WrapTextAt supports breakWords option

---

### 7. [LINEENDING_RULES.md](LINEENDING_RULES.md)
**4 line ending methods**
- ToUnixLineEndings - Convert to `\n`
- ToWindowsLineEndings - Convert to `\r\n`
- ToMacLineEndings - Convert to `\r` (legacy, obsolete)
- NormalizeLineEndings - Platform-specific

**Key Rules:**
- Always normalize to Unix first when converting to Windows (avoid `\r\r\n`)
- NormalizeLineEndings uses platform default by default
- Handle mixed line endings correctly
- Idempotent operations (calling multiple times = calling once)

---

### 8. [DATAFORMAT_RULES.md](DATAFORMAT_RULES.md)
**35+ encoding and format methods**

**CSV:**
- ToCsvField, FromCsvField
- SplitCsvLine, ToCsvLine
- ToCsv (2D array, list of lists)
- FromCsvToArray, FromCsvToList

**JSON:**
- ToJsonString, FromJsonString
- IsValidJson
- ToJsonArray (array, 2D array)
- FromJsonToArray, FromJsonToList

**XML:**
- ToXmlContent, FromXmlContent

**Base64:**
- ToBase64, FromBase64, IsValidBase64

**URL:**
- ToUrlEncoded, FromUrlEncoded

**HTML:**
- ToHtmlEncoded, FromHtmlEncoded

**Hex:**
- ToHex, FromHex, IsValidHex

**Key Rules:**
- CSV: Quote fields containing `,`, `"`, or newlines; escape `"` as `""`
- JSON: Escape control characters and quotes
- XML: Escape `&` first (avoid double-escaping)
- Base64: Use UTF-8 encoding
- URL: RFC 3986 compliance
- HTML: Prevent XSS attacks
- Hex: Uppercase by default, even-length strings

---

## 🎯 Cross-Platform Implementation Requirements

### Null Safety
All methods **MUST** validate inputs:
```
// Throws ArgumentNullException
if (value == null) throw ArgumentNullException

// OR accepts null (for IsNullOrEmpty, IsNullOrWhiteSpace)
if (value == null) return true/false
```

### Culture Awareness
String operations **MUST** use invariant culture for predictable results:
- C#: `CultureInfo.InvariantCulture`
- JavaScript: `toLocaleUpperCase('en-US')` or locale-agnostic
- Python: Standard string methods (locale-independent by default)

### Error Handling
Methods **MUST** throw on invalid input:
- Null reference errors (except nullable methods)
- Argument out of range (negative lengths, invalid indices)
- Format exceptions (invalid Base64, Hex, JSON, etc.)

### Return Values
- **Extraction methods**: Return empty array if no matches (never null)
- **Validation methods**: Return boolean (never throw on invalid format)
- **Transformation methods**: Return new string (immutable)
- **Counting methods**: Return integer >= 0

---

## 🧪 Testing Requirements

Each method requires tests for:
1. **Happy path** - Expected usage
2. **Edge cases** - Empty string, single character, very long strings
3. **Null handling** - Appropriate error or acceptance
4. **Unicode** - Non-ASCII characters
5. **Whitespace** - Spaces, tabs, newlines
6. **Special characters** - Punctuation, symbols, numbers

### Example Test Matrix
```
Method: ToUpperCase()
- Input: "hello"      → Output: "HELLO"        ✅
- Input: ""           → Output: ""             ✅
- Input: null         → Error: ArgumentNull    ✅
- Input: "café"       → Output: "CAFÉ"         ✅
- Input: "123"        → Output: "123"          ✅
```

---

## 📊 Performance Guidelines

All implementations **SHOULD**:
- Use single-pass algorithms where possible
- Avoid unnecessary allocations
- Cache compiled regex patterns
- Use platform-specific optimizations

**Complexity targets:**
- Substring operations: O(n)
- Counting operations: O(n)
- Pattern matching: O(n) with regex compilation
- Transformations: O(n)

---

## 🔄 Version Compatibility

These specifications are **versioned** with the library:
- **v1.x**: Current specification
- Breaking changes require major version bump
- New methods can be added in minor versions

---

## 🤝 Implementation Checklist

When implementing for a new platform:
- [ ] Read all 8 specification documents
- [ ] Implement all methods per specification
- [ ] Pass all behavioral test cases
- [ ] Add platform-specific optimizations
- [ ] Maintain consistent API naming
- [ ] Document platform-specific notes

---

## 📝 Notes for Implementers

### Naming Conventions
All platforms **MUST** use consistent naming:
- Method names: PascalCase (ToUpperCase, IsNullOrEmpty)
- Parameters: camelCase (value, pattern, maxLength)

### Namespace/Package Structure
All platforms **MUST** use semantic namespaces:
```
MarcusMedina.Fluent.Text.Casing
MarcusMedina.Fluent.Text.Validation
MarcusMedina.Fluent.Text.Pattern
MarcusMedina.Fluent.Text.Extraction
MarcusMedina.Fluent.Text.Counting
MarcusMedina.Fluent.Text.Manipulation
MarcusMedina.Fluent.Text.LineEndings
MarcusMedina.Fluent.Text.DataFormat
```

**JavaScript/TypeScript:**
```typescript
import { ToUpperCase } from '@marcusmedina/fluent-text/casing';
import { IsNullOrEmpty } from '@marcusmedina/fluent-text/validation';
```

**Python:**
```python
from mm_fluent_text.casing import to_upper_case
from mm_fluent_text.validation import is_null_or_empty
```

**C#:**
```csharp
using MarcusMedina.Fluent.Text.Casing;
using MarcusMedina.Fluent.Text.Validation;
```

---

## 🎓 Learning Resources

- [String Handling Best Practices](https://docs.microsoft.com/en-us/dotnet/standard/base-types/best-practices-strings)
- [Unicode Standard](https://unicode.org/standard/standard.html)
- [CSV RFC 4180](https://tools.ietf.org/html/rfc4180)
- [JSON RFC 8259](https://tools.ietf.org/html/rfc8259)
- [URL Encoding RFC 3986](https://tools.ietf.org/html/rfc3986)

---

**These specifications ensure consistent behavior across all FluentBuilders.Text implementations!** 🎉
