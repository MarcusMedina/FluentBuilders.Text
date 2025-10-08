# Manipulation Rules Reference

Detailed specification for all manipulation methods in `MarcusMedina.Fluent.Text.Manipulation`.

---

## Reverse()
**Rule**: Reverse the order of characters in the string.

**Signature**: `string Reverse(this string value)`

```csharp
"hello".Reverse()                       // "olleh"
"Hello World".Reverse()                 // "dlroW olleH"
"12345".Reverse()                       // "54321"
"a".Reverse()                           // "a"
"".Reverse()                            // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Single character returns same character
- Preserves whitespace and punctuation positions

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return new string(value.Reverse().ToArray());
```

---

## Shuffle()
**Rule**: Randomly shuffle the characters in the string.

**Signature**: `string Shuffle(this string value)`

```csharp
"hello".Shuffle()                       // "lhelo" (random)
"abc".Shuffle()                         // "bca" (random)
"test".Shuffle()                        // "tets" (random)
"".Shuffle()                            // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Single character returns same character
- Uses `Random` class (not cryptographically secure)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
var random = new Random();
return new string(value.OrderBy(_ => random.Next()).ToArray());
```

---

## ShuffleWords()
**Rule**: Randomly shuffle the order of words (not characters).

**Signature**: `string ShuffleWords(this string value)`

```csharp
"hello world today".ShuffleWords()      // "today hello world" (random)
"one two three".ShuffleWords()          // "two one three" (random)
"single".ShuffleWords()                 // "single"
"".ShuffleWords()                       // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Preserves individual word integrity (only order changes)
- Multiple spaces collapsed to single space in output
- Empty string returns empty string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
var words = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
var random = new Random();
return string.Join(" ", words.OrderBy(_ => random.Next()));
```

---

## ShuffleSentences()
**Rule**: Randomly shuffle the order of sentences.

**Signature**: `string ShuffleSentences(this string value)`

```csharp
"First. Second. Third.".ShuffleSentences()  // "Second. Third. First." (random)
"Hi! How are you?".ShuffleSentences()       // "How are you? Hi!" (random)
```

**Sentence Terminators**: `.`, `!`, `?`

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Preserves sentence punctuation
- Trims whitespace between sentences

---

## Repeat()
**Rule**: Repeat the string **n** times.

**Signature**: `string Repeat(this string value, int count)`

```csharp
"Ha".Repeat(3)                          // "HaHaHa"
"Test".Repeat(2)                        // "TestTest"
"X".Repeat(5)                           // "XXXXX"
"test".Repeat(0)                        // ""
"test".Repeat(1)                        // "test"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `ArgumentOutOfRangeException` for negative count
- `Repeat(0)` returns empty string
- `Repeat(1)` returns original string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
return string.Concat(Enumerable.Repeat(value, count));
```

---

## Truncate()
**Rule**: Truncate string to maximum length, add ellipsis if truncated.

**Signature**: `string Truncate(this string value, int maxLength, string ellipsis = "...")`

```csharp
"This is a very long sentence".Truncate(10)         // "This is..."
"This is a very long sentence".Truncate(10, "…")    // "This is a…"
"Short".Truncate(10)                                // "Short"
"Exact".Truncate(5)                                 // "Exact"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `ArgumentOutOfRangeException` for negative maxLength
- If string shorter than maxLength, returns original
- Ellipsis included in maxLength count
- If maxLength < ellipsis.Length, returns truncated without ellipsis

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (maxLength < 0) throw new ArgumentOutOfRangeException(nameof(maxLength));

if (value.Length <= maxLength) return value;

int truncateAt = maxLength - ellipsis.Length;
if (truncateAt <= 0) return value.Substring(0, maxLength);

return value.Substring(0, truncateAt) + ellipsis;
```

---

## Mask()
**Rule**: Mask characters with a mask character (for sensitive data).

**Signature**: `string Mask(this string value, int visibleStart, int visibleEnd, char maskChar = '*')`

```csharp
"1234567890".Mask(4, 2)                 // "1234****90"
"1234567890".Mask(0, 4)                 // "******7890"
"password".Mask(0, 0, '#')              // "########"
"test".Mask(1, 1)                       // "t**t"
```

**Parameters**:
- `visibleStart`: Number of characters visible at start
- `visibleEnd`: Number of characters visible at end
- `maskChar`: Character to use for masking (default `*`)

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `ArgumentOutOfRangeException` for negative visibleStart/visibleEnd
- If `visibleStart + visibleEnd >= value.Length`, returns original string
- Minimum masked section is 0 characters

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (visibleStart < 0) throw new ArgumentOutOfRangeException(nameof(visibleStart));
if (visibleEnd < 0) throw new ArgumentOutOfRangeException(nameof(visibleEnd));

if (visibleStart + visibleEnd >= value.Length) return value;

int maskLength = value.Length - visibleStart - visibleEnd;
return value.Substring(0, visibleStart) +
       new string(maskChar, maskLength) +
       value.Substring(value.Length - visibleEnd);
```

---

## RemoveWhitespace()
**Rule**: Remove **all** whitespace characters.

**Signature**: `string RemoveWhitespace(this string value)`

```csharp
"hello world".RemoveWhitespace()        // "helloworld"
"  spaces  ".RemoveWhitespace()         // "spaces"
"a b c".RemoveWhitespace()              // "abc"
"\t\n\r".RemoveWhitespace()             // ""
```

**Whitespace Characters**: Space, tab, newline, carriage return, and all Unicode whitespace.

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Removes ALL types of whitespace (not just spaces)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
```

---

## CollapseWhitespace()
**Rule**: Replace multiple consecutive whitespace with single space.

**Signature**: `string CollapseWhitespace(this string value)`

```csharp
"hello    world".CollapseWhitespace()   // "hello world"
"  multiple   spaces  ".CollapseWhitespace()  // " multiple spaces "
"test\n\n\nlines".CollapseWhitespace()  // "test lines"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Preserves leading/trailing whitespace (collapses but doesn't trim)
- All whitespace types (space, tab, newline) collapsed to single space

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return Regex.Replace(value, @"\s+", " ");
```

---

## InsertAt()
**Rule**: Insert text at a specific position.

**Signature**: `string InsertAt(this string value, int index, string text)`

```csharp
"hello world".InsertAt(5, " beautiful")     // "hello beautiful world"
"test".InsertAt(0, "pre")                   // "pretest"
"test".InsertAt(4, "ing")                   // "testing"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value or text
- Throws `ArgumentOutOfRangeException` if index < 0 or index > value.Length
- Inserting at `value.Length` appends to end
- Empty text returns original string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
ArgumentNullException.ThrowIfNull(text);
return value.Insert(index, text);
```

---

## WrapTextAt()
**Rule**: Wrap text to fit within a maximum line length.

**Signature**: `string WrapTextAt(this string value, int maxLineLength, bool breakWords = false)`

```csharp
"This is a very long sentence that needs wrapping".WrapTextAt(20)
// "This is a very\nlong sentence that\nneeds wrapping"

"Supercalifragilisticexpialidocious".WrapTextAt(10, breakWords: true)
// "Supercalif\nragilisti\ncexpialido\ncious"
```

**Parameters**:
- `maxLineLength`: Maximum characters per line
- `breakWords`: If `true`, break words mid-word; if `false`, only break at spaces

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `ArgumentOutOfRangeException` for maxLineLength <= 0
- If `breakWords = false` and word longer than maxLineLength, word preserved on its own line

**Implementation** (simplified):
```csharp
ArgumentNullException.ThrowIfNull(value);
if (maxLineLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxLineLength));

var lines = new List<string>();
var words = value.Split(' ');
var currentLine = new StringBuilder();

foreach (var word in words)
{
    if (currentLine.Length + word.Length + 1 > maxLineLength)
    {
        if (currentLine.Length > 0)
        {
            lines.Add(currentLine.ToString());
            currentLine.Clear();
        }

        if (breakWords && word.Length > maxLineLength)
        {
            // Break word into chunks
            for (int i = 0; i < word.Length; i += maxLineLength)
            {
                lines.Add(word.Substring(i, Math.Min(maxLineLength, word.Length - i)));
            }
        }
        else
        {
            currentLine.Append(word);
        }
    }
    else
    {
        if (currentLine.Length > 0) currentLine.Append(' ');
        currentLine.Append(word);
    }
}

if (currentLine.Length > 0) lines.Add(currentLine.ToString());
return string.Join("\n", lines);
```

---

## Test Matrix

| Method | Input | Arguments | Result |
|--------|-------|-----------|--------|
| Reverse | `"hello"` | - | `"olleh"` |
| Repeat | `"Ha"` | `3` | `"HaHaHa"` |
| Truncate | `"Long sentence"` | `10` | `"Long se..."` |
| Mask | `"1234567890"` | `4, 2` | `"1234****90"` |
| RemoveWhitespace | `"hello world"` | - | `"helloworld"` |
| CollapseWhitespace | `"hello    world"` | - | `"hello world"` |
| InsertAt | `"hello world"` | `5, " big"` | `"hello big world"` |
| WrapTextAt | `"Long sentence"` | `5` | `"Long\nsente\nnce"` (with breakWords) |

---

## Performance Considerations

- **Reverse()**: O(n) - Character array allocation
- **Shuffle()**: O(n log n) - LINQ OrderBy sorting
- **Repeat()**: O(n × count) - String concatenation
- **Truncate()**: O(n) - Substring operation
- **Mask()**: O(n) - String concatenation
- **RemoveWhitespace()**: O(n) - LINQ Where + char array
- **CollapseWhitespace()**: O(n) - Regex replacement
- **WrapTextAt()**: O(n) - StringBuilder operations

**Optimization**: Use `StringBuilder` for repeated concatenations.

---

This specification ensures consistent manipulation behavior across all methods!
