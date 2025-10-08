# Pattern Matching Rules Reference

Detailed specification for all pattern matching methods in `MarcusMedina.Fluent.Text.Pattern`.

---

## IsLike()
**Rule**: SQL-style LIKE pattern matching with wildcards.

**Signature**: `bool IsLike(this string value, string pattern)`

**Wildcards**:
- `%` - Matches zero or more characters (like `*` in glob)
- `_` - Matches exactly one character (like `?` in glob)

```csharp
"test".IsLike("te%")        // true (starts with "te")
"test".IsLike("%st")        // true (ends with "st")
"test".IsLike("%es%")       // true (contains "es")
"test".IsLike("t_st")       // true (_ matches 'e')
"test".IsLike("t__t")       // true (__ matches 'es')
"test".IsLike("test")       // true (exact match)
"test".IsLike("TEST")       // false (case-sensitive by default)

"hello world".IsLike("hello%")    // true
"hello world".IsLike("%world")    // true
"hello world".IsLike("hello%world")  // true
"hello world".IsLike("h_llo%")    // true
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value or pattern
- Empty pattern `""` only matches empty string
- `%` matches empty string: `"".IsLike("%")` → `true`
- `_` does not match empty string: `"".IsLike("_")` → `false`
- Special characters in pattern are literal (no regex)

**Implementation**: Convert to regex pattern
```csharp
% → .*
_ → .
Escape other regex special chars
```

---

## ContainsText()
**Rule**: **Case-insensitive** contains check.

**Signature**: `bool ContainsText(this string value, string substring)`

```csharp
"Hello World".ContainsText("WORLD")     // true
"Hello World".ContainsText("world")     // true
"Hello World".ContainsText("HELLO")     // true
"Hello World".ContainsText("xyz")       // false
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value or substring
- Empty substring always returns `true` (standard .NET behavior)
- Uses `StringComparison.OrdinalIgnoreCase`

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
ArgumentNullException.ThrowIfNull(substring);
return value.Contains(substring, StringComparison.OrdinalIgnoreCase);
```

---

## StartsWithText()
**Rule**: **Case-insensitive** starts-with check.

**Signature**: `bool StartsWithText(this string value, string prefix)`

```csharp
"Hello World".StartsWithText("HELLO")   // true
"Hello World".StartsWithText("hello")   // true
"Hello World".StartsWithText("WORLD")   // false
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value or prefix
- Empty prefix always returns `true`
- Uses `StringComparison.OrdinalIgnoreCase`

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
ArgumentNullException.ThrowIfNull(prefix);
return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
```

---

## EndsWithText()
**Rule**: **Case-insensitive** ends-with check.

**Signature**: `bool EndsWithText(this string value, string suffix)`

```csharp
"Hello World".EndsWithText("WORLD")     // true
"Hello World".EndsWithText("world")     // true
"Hello World".EndsWithText("HELLO")     // false
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value or suffix
- Empty suffix always returns `true`
- Uses `StringComparison.OrdinalIgnoreCase`

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
ArgumentNullException.ThrowIfNull(suffix);
return value.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
```

---

## In()
**Rule**: Check if string exists in a collection.

**Signature**: `bool In(this string value, params string[] values)`
**Overload**: `bool In(this string value, IEnumerable<string> values)`

```csharp
"hello".In("hello", "world")            // true
"test".In("hello", "world")             // false
"hello".In(new[] { "hello", "world" })  // true

// Case-sensitive by default
"HELLO".In("hello", "world")            // false
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value or values collection
- Empty collection returns `false`
- Null items in collection are compared (can match if value is null)
- Uses `Enumerable.Contains()` with default equality comparer

**Case-Insensitive Variant**:
```csharp
"HELLO".In(StringComparer.OrdinalIgnoreCase, "hello", "world")  // true
```

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
ArgumentNullException.ThrowIfNull(values);
return values.Contains(value);
```

---

## Between()
**Rule**: Check if string is **alphabetically** between two values (inclusive).

**Signature**: `bool Between(this string value, string lower, string upper)`

```csharp
"bob".Between("alice", "charlie")       // true
"alice".Between("alice", "charlie")     // true (inclusive)
"charlie".Between("alice", "charlie")   // true (inclusive)
"dave".Between("alice", "charlie")      // false

// Alphabetical comparison
"banana".Between("apple", "cherry")     // true
"dog".Between("cat", "elephant")        // true
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value, lower, or upper
- Uses `StringComparer.Ordinal` (case-sensitive)
- Inclusive on both bounds
- Empty strings are valid: `"".Between("", "z")` → `true`

**Case-Insensitive Variant**:
```csharp
"BOB".Between("alice", "charlie", StringComparer.OrdinalIgnoreCase)  // true
```

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
ArgumentNullException.ThrowIfNull(lower);
ArgumentNullException.ThrowIfNull(upper);

return string.Compare(value, lower, StringComparison.Ordinal) >= 0 &&
       string.Compare(value, upper, StringComparison.Ordinal) <= 0;
```

---

## Test Matrix

### IsLike() Tests
| Input | Pattern | Result |
|-------|---------|--------|
| `"test"` | `"te%"` | ✅ true |
| `"test"` | `"%st"` | ✅ true |
| `"test"` | `"%es%"` | ✅ true |
| `"test"` | `"t_st"` | ✅ true |
| `"test"` | `"t__t"` | ✅ true |
| `"test"` | `"test"` | ✅ true |
| `"test"` | `"TEST"` | ❌ false |
| `"test"` | `"xyz%"` | ❌ false |
| `""` | `"%"` | ✅ true |
| `""` | `"_"` | ❌ false |

### ContainsText/StartsWithText/EndsWithText Tests
| Input | Method | Argument | Result |
|-------|--------|----------|--------|
| `"Hello World"` | ContainsText | `"WORLD"` | ✅ true |
| `"Hello World"` | StartsWithText | `"HELLO"` | ✅ true |
| `"Hello World"` | EndsWithText | `"WORLD"` | ✅ true |
| `"Hello World"` | ContainsText | `"xyz"` | ❌ false |
| `"Hello World"` | StartsWithText | `"WORLD"` | ❌ false |
| `"Hello World"` | EndsWithText | `"HELLO"` | ❌ false |

### In() Tests
| Input | Collection | Result |
|-------|------------|--------|
| `"hello"` | `["hello", "world"]` | ✅ true |
| `"test"` | `["hello", "world"]` | ❌ false |
| `"HELLO"` | `["hello", "world"]` | ❌ false (case-sensitive) |
| `"hello"` | `[]` | ❌ false |

### Between() Tests
| Input | Lower | Upper | Result |
|-------|-------|-------|--------|
| `"bob"` | `"alice"` | `"charlie"` | ✅ true |
| `"alice"` | `"alice"` | `"charlie"` | ✅ true |
| `"charlie"` | `"alice"` | `"charlie"` | ✅ true |
| `"dave"` | `"alice"` | `"charlie"` | ❌ false |
| `"BOB"` | `"alice"` | `"charlie"` | ❌ false (case-sensitive) |

---

## Usage Recommendations

**IsLike()**:
- User input filtering/searching
- Simple wildcard matching without full regex
- Configuration file patterns

**ContainsText/StartsWithText/EndsWithText()**:
- Case-insensitive string searching
- User-friendly searches (ignore case)
- File/folder name matching

**In()**:
- Validation against allowed values
- Switch-case alternative
- Configuration validation

**Between()**:
- Alphabetical range validation
- Name filtering (A-M, N-Z)
- Version string comparisons

---

## Performance Considerations

- **IsLike()**: O(n) - Regex compilation cached, execution proportional to input length
- **ContainsText/StartsWithText/EndsWithText()**: O(n) - BCL optimized for OrdinalIgnoreCase
- **In()**: O(n) - Linear search through collection
- **Between()**: O(1) - Two string comparisons

**Optimization**: For repeated `In()` checks with the same collection, convert to `HashSet<string>` first.

---

This specification ensures consistent pattern matching behavior across all methods!
