# Validation Rules Reference

**⚠️ C# Note:** Use built-in `string.IsNullOrEmpty()` and `string.IsNullOrWhiteSpace()` instead of extension methods. This spec covers **IsEmpty()** and **IsWhiteSpace()** which C# doesn't provide.

Detailed specification for validation methods in FluentBuilders.Text.

---

## IsEmpty()
**Rule**: Check if string has zero length.

**Signature**: `bool IsEmpty(this string value)`

```csharp
"".IsEmpty()        // true
"hello".IsEmpty()   // false
" ".IsEmpty()       // false (whitespace is not empty)
```

**Edge Cases**:
- Throws `ArgumentNullException` for null input
- Returns `true` for `string.Empty`
- Returns `false` for whitespace-only strings

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Length == 0;
```

---

## IsWhiteSpace()
**Rule**: Check if string is **non-empty** and contains **only** whitespace.

**Signature**: `bool IsWhiteSpace(this string value)`

```csharp
"   ".IsWhiteSpace()    // true
"\t\n".IsWhiteSpace()   // true
"".IsWhiteSpace()       // false (empty, not whitespace)
"hello".IsWhiteSpace()  // false
" a ".IsWhiteSpace()    // false (contains non-whitespace)
```

**Key Difference from string.IsNullOrWhiteSpace()**:
- `IsWhiteSpace()` returns `false` for empty string and throws on null
- `string.IsNullOrWhiteSpace()` returns `true` for empty string and null

**Edge Cases**:
- Throws `ArgumentNullException` for null input
- Returns `false` for empty string (must be non-empty to return true)
- Returns `true` only if **all** characters are whitespace

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Length > 0 && value.All(char.IsWhiteSpace);
```

---

## Test Matrix

Each validation method should be tested with:

1. **Null**: `null` (should throw or return true depending on signature)
2. **Empty**: `""`
3. **Single space**: `" "`
4. **Multiple spaces**: `"   "`
5. **Tab**: `"\t"`
6. **Newline**: `"\n"`
7. **Mixed whitespace**: `" \t\n "`
8. **Single character**: `"a"`
9. **Word**: `"hello"`
10. **Whitespace + content**: `" hello "`

---

## Truth Table

| Input | IsEmpty() | IsWhiteSpace() | string.IsNullOrEmpty() | string.IsNullOrWhiteSpace() |
|-------|-----------|----------------|------------------------|----------------------------|
| `null` | ❌ Throws | ❌ Throws | ✅ true | ✅ true |
| `""` | ✅ true | ❌ false | ✅ true | ✅ true |
| `" "` | ❌ false | ✅ true | ❌ false | ✅ true |
| `"   "` | ❌ false | ✅ true | ❌ false | ✅ true |
| `"\t"` | ❌ false | ✅ true | ❌ false | ✅ true |
| `"\n"` | ❌ false | ✅ true | ❌ false | ✅ true |
| `" \t\n "` | ❌ false | ✅ true | ❌ false | ✅ true |
| `"a"` | ❌ false | ❌ false | ❌ false | ❌ false |
| `"hello"` | ❌ false | ❌ false | ❌ false | ❌ false |
| `" a "` | ❌ false | ❌ false | ❌ false | ❌ false |

---

## Usage Recommendations

**When to use IsEmpty()**:
- When you specifically need to check for zero-length strings
- When you've already verified input is not null
- When you want to distinguish between empty and whitespace
- Extension method alternative to `value.Length == 0`

**When to use IsWhiteSpace()**:
- When you need to specifically detect whitespace-only strings (not empty or null)
- When you need to distinguish between empty and whitespace
- Sanitization/cleanup logic
- More specific than `string.IsNullOrWhiteSpace()`

**When to use string.IsNullOrEmpty()** (C# built-in):
- Most common validation scenario
- When null and empty should be treated the same
- Input validation for user-provided strings

**When to use string.IsNullOrWhiteSpace()** (C# built-in):
- When validating user input that shouldn't be "blank"
- Form validation
- When whitespace-only input is meaningless

---

## Performance Considerations

- `IsEmpty()`: O(1) - Just checks `Length` property
- `IsWhiteSpace()`: O(n) - Must scan entire string with LINQ
- `string.IsNullOrEmpty()`: O(1) - BCL optimized
- `string.IsNullOrWhiteSpace()`: O(n) - Must scan entire string

**Optimization Note**: For very long strings, `string.IsNullOrWhiteSpace()` is more efficient than `IsWhiteSpace()` because it uses BCL's optimized implementation.

---

This specification ensures consistent validation behavior across all methods!
