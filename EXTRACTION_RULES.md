# Extraction Rules Reference

Detailed specification for all extraction methods in `MarcusMedina.Fluent.Text.Extraction`.

---

## Basic Substring Extraction

### Left()
**Rule**: Extract **n** characters from the **left** (start) of string.

**Signature**: `string Left(this string value, int length)`

```csharp
"hello world".Left(5)       // "hello"
"test".Left(2)              // "te"
"test".Left(10)             // "test" (length clamped to string length)
"test".Left(0)              // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `ArgumentOutOfRangeException` for negative length
- If `length > value.Length`, returns entire string (safe clamp)
- `Left(0)` returns empty string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
return value.Substring(0, Math.Min(length, value.Length));
```

---

### Right()
**Rule**: Extract **n** characters from the **right** (end) of string.

**Signature**: `string Right(this string value, int length)`

```csharp
"hello world".Right(5)      // "world"
"test".Right(2)             // "st"
"test".Right(10)            // "test" (length clamped to string length)
"test".Right(0)             // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `ArgumentOutOfRangeException` for negative length
- If `length > value.Length`, returns entire string (safe clamp)
- `Right(0)` returns empty string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
int actualLength = Math.Min(length, value.Length);
return value.Substring(value.Length - actualLength, actualLength);
```

---

### Mid()
**Rule**: Extract **length** characters starting from **startIndex**.

**Signature**: `string Mid(this string value, int startIndex, int length)`

```csharp
"hello world".Mid(6, 5)     // "world"
"hello world".Mid(0, 5)     // "hello"
"hello world".Mid(3, 5)     // "lo wo"
"test".Mid(0, 10)           // "test" (length clamped)
"test".Mid(2, 10)           // "st" (length clamped from startIndex)
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `ArgumentOutOfRangeException` for negative startIndex or length
- Throws `ArgumentOutOfRangeException` if startIndex >= value.Length
- If `length` extends beyond string, returns from startIndex to end

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
if (startIndex >= value.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));

int actualLength = Math.Min(length, value.Length - startIndex);
return value.Substring(startIndex, actualLength);
```

---

## Data Extraction (Regex-Based)

### ExtractEmails()
**Rule**: Extract all email addresses from string.

**Signature**: `string[] ExtractEmails(this string value)`

**Pattern**: `\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b`

```csharp
"Contact: user@test.com".ExtractEmails()
// ["user@test.com"]

"Emails: john@example.com, jane@test.org".ExtractEmails()
// ["john@example.com", "jane@test.org"]

"No emails here".ExtractEmails()
// []
```

**Valid Formats**:
- `user@example.com`
- `first.last@example.com`
- `user+tag@example.co.uk`
- `user_name@example-domain.com`

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Returns empty array if no emails found
- Basic validation (not RFC 5322 compliant)

---

### ExtractUrls()
**Rule**: Extract all URLs (http/https) from string.

**Signature**: `string[] ExtractUrls(this string value)`

**Pattern**: `https?://[^\s]+`

```csharp
"Visit https://example.com".ExtractUrls()
// ["https://example.com"]

"Sites: http://test.com and https://example.org/path".ExtractUrls()
// ["http://test.com", "https://example.org/path"]

"No URLs here".ExtractUrls()
// []
```

**Valid Formats**:
- `http://example.com`
- `https://example.com/path`
- `https://example.com/path?query=1`
- `https://example.com:8080/path#anchor`

---

### ExtractPhoneNumbers()
**Rule**: Extract phone numbers in common formats.

**Signature**: `string[] ExtractPhoneNumbers(this string value)`

**Pattern**: Matches formats like:
- `123-456-7890`
- `(123) 456-7890`
- `123.456.7890`
- `+1 123-456-7890`

```csharp
"Call 123-456-7890".ExtractPhoneNumbers()
// ["123-456-7890"]

"Phone: (555) 123-4567 or 555.123.4567".ExtractPhoneNumbers()
// ["(555) 123-4567", "555.123.4567"]
```

---

### ExtractDates()
**Rule**: Extract date strings in common formats.

**Signature**: `string[] ExtractDates(this string value)`

**Pattern**: Matches formats like:
- `2024-10-05`
- `10/05/2024`
- `05-10-2024`
- `October 5, 2024`

```csharp
"Meeting on 2024-10-05".ExtractDates()
// ["2024-10-05"]

"Dates: 10/05/2024 and 2024-10-05".ExtractDates()
// ["10/05/2024", "2024-10-05"]
```

---

### ExtractNumbers()
**Rule**: Extract all numeric values (integers and decimals).

**Signature**: `string[] ExtractNumbers(this string value)`

**Pattern**: `\d+\.?\d*`

```csharp
"Price: $19.99".ExtractNumbers()
// ["19.99"]

"Values: 42, 3.14, 100".ExtractNumbers()
// ["42", "3.14", "100"]
```

---

### ExtractHashtags()
**Rule**: Extract all hashtags (#word).

**Signature**: `string[] ExtractHashtags(this string value)`

**Pattern**: `#\w+`

```csharp
"Love #coding and #dotnet!".ExtractHashtags()
// ["#coding", "#dotnet"]

"No hashtags here".ExtractHashtags()
// []
```

---

### ExtractMentions()
**Rule**: Extract all mentions (@username).

**Signature**: `string[] ExtractMentions(this string value)`

**Pattern**: `@\w+`

```csharp
"Thanks @john and @jane!".ExtractMentions()
// ["@john", "@jane"]

"No mentions here".ExtractMentions()
// []
```

---

## Advanced Extraction

### ExtractBetween()
**Rule**: Extract all text between two delimiters.

**Signature**: `string[] ExtractBetween(this string value, string start, string end)`

```csharp
"Text [inside] brackets".ExtractBetween("[", "]")
// ["inside"]

"Multiple [first] and [second] brackets".ExtractBetween("[", "]")
// ["first", "second"]

"<tag>content</tag>".ExtractBetween("<tag>", "</tag>")
// ["content"]
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value, start, or end
- Returns empty array if no matches
- Non-greedy matching (shortest match)

---

### ExtractWordsContaining()
**Rule**: Extract all words containing a specific substring.

**Signature**: `string[] ExtractWordsContaining(this string value, string substring)`

```csharp
"hello world wonderful".ExtractWordsContaining("or")
// ["world", "wonderful"]

"test testing tester".ExtractWordsContaining("test")
// ["test", "testing", "tester"]
```

---

### ExtractWordsStartingWith()
**Rule**: Extract all words starting with a specific prefix.

**Signature**: `string[] ExtractWordsStartingWith(this string value, string prefix)`

```csharp
"test testing tester".ExtractWordsStartingWith("test")
// ["test", "testing", "tester"]

"hello world".ExtractWordsStartingWith("wo")
// ["world"]
```

---

### ExtractWordsEndingWith()
**Rule**: Extract all words ending with a specific suffix.

**Signature**: `string[] ExtractWordsEndingWith(this string value, string suffix)`

```csharp
"testing walking talking".ExtractWordsEndingWith("ing")
// ["testing", "walking", "talking"]

"test tester".ExtractWordsEndingWith("er")
// ["tester"]
```

---

### ExtractWordsOfLength()
**Rule**: Extract all words of a specific length.

**Signature**: `string[] ExtractWordsOfLength(this string value, int length)`

```csharp
"the quick brown fox".ExtractWordsOfLength(3)
// ["the", "fox"]

"hello world".ExtractWordsOfLength(5)
// ["hello", "world"]
```

---

### ExtractAllWords()
**Rule**: Extract all words (separated by whitespace).

**Signature**: `string[] ExtractAllWords(this string value)`

```csharp
"hello world test".ExtractAllWords()
// ["hello", "world", "test"]

"  multiple   spaces  ".ExtractAllWords()
// ["multiple", "spaces"]
```

**Word Definition**: Separated by whitespace, punctuation removed.

---

### ExtractAllSentences()
**Rule**: Extract all sentences (ending with `.`, `!`, `?`).

**Signature**: `string[] ExtractAllSentences(this string value)`

```csharp
"Hello world. How are you?".ExtractAllSentences()
// ["Hello world", "How are you"]

"First! Second? Third.".ExtractAllSentences()
// ["First", "Second", "Third"]
```

---

## Test Matrix

### Substring Methods
| Method | Input | Arguments | Result |
|--------|-------|-----------|--------|
| Left | `"hello world"` | `5` | `"hello"` |
| Right | `"hello world"` | `5` | `"world"` |
| Mid | `"hello world"` | `6, 5` | `"world"` |
| Left | `"test"` | `10` | `"test"` (clamped) |
| Right | `"test"` | `10` | `"test"` (clamped) |
| Mid | `"test"` | `2, 10` | `"st"` (clamped) |

---

## Performance Considerations

- **Left/Right/Mid**: O(n) - Substring operations
- **Regex-based extraction**: O(n) - Regex match proportional to input length
- **Word extraction**: O(n) - String split and filter

**Optimization**: Regex patterns are compiled and cached for repeated use.

---

This specification ensures consistent extraction behavior across all methods!
