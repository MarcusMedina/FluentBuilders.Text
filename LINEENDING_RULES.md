# Line Ending Rules Reference

Detailed specification for all line ending methods in `MarcusMedina.Fluent.Text.LineEndings`.

---

## Line Ending Types

**Unix/Linux** (`\n`):
- Character: Line Feed (LF)
- Hexadecimal: `0x0A`
- Used by: Linux, macOS, Unix systems

**Windows** (`\r\n`):
- Characters: Carriage Return + Line Feed (CRLF)
- Hexadecimal: `0x0D 0x0A`
- Used by: Windows, DOS, OS/2

**Mac Legacy** (`\r`):
- Character: Carriage Return (CR)
- Hexadecimal: `0x0D`
- Used by: Classic Mac OS (pre-OS X)
- **Obsolete**: Modern macOS uses Unix (`\n`)

---

## ToUnixLineEndings()
**Rule**: Convert all line endings to Unix format (`\n`).

**Signature**: `string ToUnixLineEndings(this string value)`

```csharp
"line1\r\nline2".ToUnixLineEndings()                // "line1\nline2"
"line1\r\nline2\r\nline3".ToUnixLineEndings()       // "line1\nline2\nline3"
"line1\rline2".ToUnixLineEndings()                  // "line1\nline2"
"line1\nline2".ToUnixLineEndings()                  // "line1\nline2" (unchanged)
"mixed\r\nlines\nhere".ToUnixLineEndings()          // "mixed\nlines\nhere"
```

**Conversion**:
- `\r\n` → `\n` (Windows to Unix)
- `\r` → `\n` (Mac legacy to Unix)
- `\n` → `\n` (Unix unchanged)

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Handles mixed line endings correctly
- Idempotent: Calling multiple times has same result as once

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value
    .Replace("\r\n", "\n")  // Windows → Unix (must be first!)
    .Replace("\r", "\n");   // Mac → Unix
```

**Important**: Replace `\r\n` before `\r` to avoid double conversion!

---

## ToWindowsLineEndings()
**Rule**: Convert all line endings to Windows format (`\r\n`).

**Signature**: `string ToWindowsLineEndings(this string value)`

```csharp
"line1\nline2".ToWindowsLineEndings()               // "line1\r\nline2"
"line1\nline2\nline3".ToWindowsLineEndings()        // "line1\r\nline2\r\nline3"
"line1\rline2".ToWindowsLineEndings()               // "line1\r\nline2"
"line1\r\nline2".ToWindowsLineEndings()             // "line1\r\nline2" (unchanged)
"mixed\nlines\rhere".ToWindowsLineEndings()         // "mixed\r\nlines\r\nhere"
```

**Conversion**:
- `\n` → `\r\n` (Unix to Windows)
- `\r` → `\r\n` (Mac legacy to Windows)
- `\r\n` → `\r\n` (Windows unchanged)

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Handles mixed line endings correctly
- Must normalize to Unix first to avoid `\r\r\n`
- Idempotent: Calling multiple times has same result as once

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value
    .Replace("\r\n", "\n")  // Normalize to Unix first
    .Replace("\r", "\n")    // Mac → Unix
    .Replace("\n", "\r\n"); // Unix → Windows
```

**Why normalize first?**
- Without normalization: `"line\r\nline"` → `"line\r\r\nline"` ❌
- With normalization: `"line\r\nline"` → `"line\nline"` → `"line\r\nline"` ✅

---

## ToMacLineEndings()
**Rule**: Convert all line endings to Mac legacy format (`\r`).

**Signature**: `string ToMacLineEndings(this string value)`

```csharp
"line1\nline2".ToMacLineEndings()                   // "line1\rline2"
"line1\r\nline2".ToMacLineEndings()                 // "line1\rline2"
"line1\rline2".ToMacLineEndings()                   // "line1\rline2" (unchanged)
"mixed\r\nlines\nhere".ToMacLineEndings()           // "mixed\rlines\rhere"
```

**Conversion**:
- `\r\n` → `\r` (Windows to Mac)
- `\n` → `\r` (Unix to Mac)
- `\r` → `\r` (Mac unchanged)

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Handles mixed line endings correctly
- **Note**: This format is obsolete (modern macOS uses Unix)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value
    .Replace("\r\n", "\r")  // Windows → Mac (must be first!)
    .Replace("\n", "\r");   // Unix → Mac
```

---

## NormalizeLineEndings()
**Rule**: Convert all line endings to platform-specific format (`Environment.NewLine`).

**Signature**: `string NormalizeLineEndings(this string value)`
**Overload**: `string NormalizeLineEndings(this string value, string targetLineEnding)`

```csharp
// Platform-specific (automatic)
"line1\nline2".NormalizeLineEndings()
// Windows: "line1\r\nline2"
// Unix/macOS: "line1\nline2"

// Explicit target
"line1\r\nline2".NormalizeLineEndings("\n")         // "line1\nline2"
"line1\nline2".NormalizeLineEndings("\r\n")         // "line1\r\nline2"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Uses `Environment.NewLine` by default
- Custom target ending supported via overload

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);

// Normalize to Unix first (single format)
string normalized = value
    .Replace("\r\n", "\n")
    .Replace("\r", "\n");

// Convert to target format
string targetEnding = targetLineEnding ?? Environment.NewLine;
if (targetEnding == "\n") return normalized;
return normalized.Replace("\n", targetEnding);
```

---

## Test Matrix

### ToUnixLineEndings()
| Input | Output |
|-------|--------|
| `"line1\r\nline2"` | `"line1\nline2"` |
| `"line1\rline2"` | `"line1\nline2"` |
| `"line1\nline2"` | `"line1\nline2"` |
| `"mixed\r\nlines\nhere\r"` | `"mixed\nlines\nhere\n"` |
| `""` | `""` |

### ToWindowsLineEndings()
| Input | Output |
|-------|--------|
| `"line1\nline2"` | `"line1\r\nline2"` |
| `"line1\rline2"` | `"line1\r\nline2"` |
| `"line1\r\nline2"` | `"line1\r\nline2"` |
| `"mixed\nlines\rhere"` | `"mixed\r\nlines\r\nhere"` |
| `""` | `""` |

### ToMacLineEndings()
| Input | Output |
|-------|--------|
| `"line1\nline2"` | `"line1\rline2"` |
| `"line1\r\nline2"` | `"line1\rline2"` |
| `"line1\rline2"` | `"line1\rline2"` |
| `"mixed\r\nlines\n"` | `"mixed\rlines\r"` |
| `""` | `""` |

---

## Conversion Table

| From | To Unix (`\n`) | To Windows (`\r\n`) | To Mac (`\r`) |
|------|----------------|---------------------|---------------|
| `\n` | `\n` | `\r\n` | `\r` |
| `\r\n` | `\n` | `\r\n` | `\r` |
| `\r` | `\n` | `\r\n` | `\r` |

---

## Common Use Cases

### Reading Cross-Platform Files
```csharp
// Normalize to Unix for consistent processing
string content = File.ReadAllText("data.txt").ToUnixLineEndings();
```

### Preparing for HTTP Transmission
```csharp
// HTTP spec requires CRLF for headers
string httpHeaders = headers.ToWindowsLineEndings();
```

### Git Repository Handling
```csharp
// Git internally uses LF (Unix)
string gitContent = content.ToUnixLineEndings();
```

### Legacy System Integration
```csharp
// Rare: Communicating with classic Mac systems
string macFormat = content.ToMacLineEndings();
```

---

## Edge Cases & Special Scenarios

### Empty Lines
```csharp
"line1\n\nline3".ToWindowsLineEndings()     // "line1\r\n\r\nline3"
// Empty lines preserved
```

### Trailing Newlines
```csharp
"line1\nline2\n".ToWindowsLineEndings()     // "line1\r\nline2\r\n"
// Trailing newline preserved
```

### Multiple Consecutive Newlines
```csharp
"line1\n\n\nline2".ToWindowsLineEndings()   // "line1\r\n\r\n\r\nline2"
// All newlines converted independently
```

### Mixed Line Endings
```csharp
"line1\r\nline2\nline3\rline4".ToUnixLineEndings()
// "line1\nline2\nline3\nline4"
// All variants normalized to Unix
```

---

## Performance Considerations

- **ToUnixLineEndings()**: O(n) - Two string replacements
- **ToWindowsLineEndings()**: O(n) - Three string replacements (normalize first)
- **ToMacLineEndings()**: O(n) - Two string replacements
- **NormalizeLineEndings()**: O(n) - Three string replacements

**Optimization**: Use `StringBuilder` for large files or repeated conversions.

---

## Cross-Platform Best Practices

1. **Store as Unix internally** - Use `\n` for consistency
2. **Convert on I/O boundary** - Convert to platform-specific when reading/writing
3. **Git configuration** - Use `.gitattributes` to enforce line endings
4. **HTTP compliance** - Use `\r\n` for HTTP headers (RFC 2616)
5. **Don't use Mac legacy** - `\r` format is obsolete

---

## Platform Detection

```csharp
// Detect current platform's line ending
string platformEnding = Environment.NewLine;

// Windows: "\r\n"
// Unix/Linux: "\n"
// macOS: "\n"
```

---

This specification ensures consistent line ending behavior across all platforms!
