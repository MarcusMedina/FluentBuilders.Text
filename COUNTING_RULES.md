# Counting Rules Reference

Detailed specification for all counting methods in `MarcusMedina.Fluent.Text.Counting`.

---

## CountWords()
**Rule**: Count the number of words (whitespace-separated).

**Signature**: `int CountWords(this string value)`

```csharp
"hello world".CountWords()              // 2
"one two three".CountWords()            // 3
"  multiple   spaces  ".CountWords()    // 2
"".CountWords()                         // 0
"singleword".CountWords()               // 1
```

**Word Definition**: Separated by any whitespace character (space, tab, newline).

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns `0`
- Multiple consecutive spaces count as single separator
- Leading/trailing whitespace ignored

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Split(new[] { ' ', '\t', '\r', '\n' },
    StringSplitOptions.RemoveEmptyEntries).Length;
```

---

## CountSentences()
**Rule**: Count the number of sentences (ending with `.`, `!`, `?`).

**Signature**: `int CountSentences(this string value)`

```csharp
"Hello world.".CountSentences()         // 1
"Hi! How are you?".CountSentences()     // 2
"One. Two! Three?".CountSentences()     // 3
"No punctuation".CountSentences()       // 0
"".CountSentences()                     // 0
```

**Sentence Terminators**:
- `.` (period)
- `!` (exclamation)
- `?` (question mark)

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Multiple punctuation counted separately: `"Hi!!!"` → 3
- Text without terminators returns `0`

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(c => c == '.' || c == '!' || c == '?');
```

---

## CountVowels()
**Rule**: Count the number of vowels (a, e, i, o, u - case-insensitive).

**Signature**: `int CountVowels(this string value)`

```csharp
"hello".CountVowels()                   // 2 (e, o)
"HELLO".CountVowels()                   // 2 (E, O)
"xyz".CountVowels()                     // 0
"aeiou".CountVowels()                   // 5
"".CountVowels()                        // 0
```

**Vowels**: `a`, `e`, `i`, `o`, `u` (uppercase and lowercase)

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Case-insensitive counting
- Non-alphabetic characters ignored
- Y is NOT counted as vowel

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(c => "aeiouAEIOU".Contains(c));
```

---

## CountConsonants()
**Rule**: Count the number of consonants (letters that are not vowels).

**Signature**: `int CountConsonants(this string value)`

```csharp
"hello".CountConsonants()               // 3 (h, l, l)
"HELLO".CountConsonants()               // 3 (H, L, L)
"aeiou".CountConsonants()               // 0
"xyz123".CountConsonants()              // 3 (x, y, z)
"".CountConsonants()                    // 0
```

**Consonants**: Any letter that is not a vowel (a, e, i, o, u).

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Case-insensitive counting
- Non-alphabetic characters ignored
- Y IS counted as consonant

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(c => char.IsLetter(c) && !"aeiouAEIOU".Contains(c));
```

---

## CountOccurrences() - Character
**Rule**: Count occurrences of a specific character.

**Signature**: `int CountOccurrences(this string value, char target)`

```csharp
"hello".CountOccurrences('l')           // 2
"hello".CountOccurrences('h')           // 1
"hello".CountOccurrences('x')           // 0
"HELLO".CountOccurrences('l')           // 0 (case-sensitive)
"".CountOccurrences('a')                // 0
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Case-sensitive matching
- Returns `0` if character not found

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(c => c == target);
```

---

## CountOccurrences() - Substring
**Rule**: Count occurrences of a substring.

**Signature**: `int CountOccurrences(this string value, string substring)`

```csharp
"hello world".CountOccurrences("l")     // 3
"hello world".CountOccurrences("o")     // 2
"abababab".CountOccurrences("ab")       // 4
"test".CountOccurrences("TEST")         // 0 (case-sensitive)
"".CountOccurrences("test")             // 0
```

**Overlapping Matches**: Non-overlapping by default
```csharp
"aaa".CountOccurrences("aa")            // 1 (not 2)
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value or substring
- Case-sensitive matching
- Empty substring throws `ArgumentException`
- Non-overlapping count

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
ArgumentNullException.ThrowIfNull(substring);
if (substring.Length == 0)
    throw new ArgumentException("Substring cannot be empty");

int count = 0;
int index = 0;
while ((index = value.IndexOf(substring, index)) != -1)
{
    count++;
    index += substring.Length;  // Non-overlapping
}
return count;
```

---

## CountLines()
**Rule**: Count the number of lines (based on newline characters).

**Signature**: `int CountLines(this string value)`

```csharp
"line1\nline2".CountLines()             // 2
"line1\r\nline2\r\nline3".CountLines()  // 3
"singleline".CountLines()               // 1
"\n\n\n".CountLines()                   // 4 (empty lines counted)
"".CountLines()                         // 1 (empty string is one line)
```

**Line Terminators**:
- `\n` (LF - Unix)
- `\r\n` (CRLF - Windows)
- `\r` (CR - Mac legacy)

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns `1` (one empty line)
- Trailing newline does NOT add extra line
- Mixed line endings handled correctly

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (value.Length == 0) return 1;
return value.Split(new[] { "\r\n", "\r", "\n" },
    StringSplitOptions.None).Length;
```

---

## CountDigits()
**Rule**: Count the number of digit characters (0-9).

**Signature**: `int CountDigits(this string value)`

```csharp
"hello123".CountDigits()                // 3
"abc".CountDigits()                     // 0
"2024-10-05".CountDigits()              // 8
"".CountDigits()                        // 0
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Only counts `0-9`
- Ignores negative signs, decimals, etc.

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(char.IsDigit);
```

---

## CountLetters()
**Rule**: Count the number of letter characters (a-z, A-Z).

**Signature**: `int CountLetters(this string value)`

```csharp
"hello123".CountLetters()               // 5
"Hello World!".CountLetters()           // 10
"123".CountLetters()                    // 0
"".CountLetters()                       // 0
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Counts both uppercase and lowercase
- Unicode letters included (café → 4)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(char.IsLetter);
```

---

## CountUppercase()
**Rule**: Count the number of uppercase letters.

**Signature**: `int CountUppercase(this string value)`

```csharp
"Hello World".CountUppercase()          // 2 (H, W)
"HELLO".CountUppercase()                // 5
"hello".CountUppercase()                // 0
"Hello123".CountUppercase()             // 1
"".CountUppercase()                     // 0
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Only counts letters (digits/symbols ignored)
- Unicode uppercase letters included

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(char.IsUpper);
```

---

## CountLowercase()
**Rule**: Count the number of lowercase letters.

**Signature**: `int CountLowercase(this string value)`

```csharp
"Hello World".CountLowercase()          // 8 (e, l, l, o, o, r, l, d)
"HELLO".CountLowercase()                // 0
"hello".CountLowercase()                // 5
"Hello123".CountLowercase()             // 4
"".CountLowercase()                     // 0
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Only counts letters (digits/symbols ignored)
- Unicode lowercase letters included

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Count(char.IsLower);
```

---

## Test Matrix

| Method | Input | Result | Notes |
|--------|-------|--------|-------|
| CountWords | `"hello world"` | `2` | |
| CountWords | `"  multiple   spaces  "` | `2` | Whitespace normalized |
| CountSentences | `"Hi! How are you?"` | `2` | |
| CountVowels | `"hello"` | `2` | e, o |
| CountConsonants | `"hello"` | `3` | h, l, l |
| CountOccurrences (char) | `"hello"`, `'l'` | `2` | |
| CountOccurrences (string) | `"abababab"`, `"ab"` | `4` | Non-overlapping |
| CountLines | `"line1\nline2"` | `2` | |
| CountDigits | `"hello123"` | `3` | |
| CountLetters | `"hello123"` | `5` | |
| CountUppercase | `"Hello World"` | `2` | H, W |
| CountLowercase | `"Hello World"` | `8` | |

---

## Performance Considerations

- **CountWords()**: O(n) - String split operation
- **CountSentences()**: O(n) - Single pass character count
- **CountVowels/Consonants()**: O(n) - Single pass with character check
- **CountOccurrences()**: O(n) - LINQ Count or IndexOf loop
- **CountLines()**: O(n) - String split operation
- **CountDigits/Letters/Uppercase/Lowercase()**: O(n) - LINQ Count with predicate

**Optimization**: All methods are single-pass algorithms with minimal overhead.

---

## Usage Recommendations

**Word/Sentence Counting**:
- Text statistics
- Reading time estimation
- Content analysis

**Character Counting**:
- Password strength validation
- Text complexity metrics
- Data validation

**Line Counting**:
- File analysis
- Log processing
- Multi-line text handling

---

This specification ensures consistent counting behavior across all methods!
