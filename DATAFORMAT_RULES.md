# Data Format & Encoding Rules Reference

Detailed specification for all data format and encoding methods in `MarcusMedina.Fluent.Text.DataFormat`.

---

## CSV Operations

### ToCsvField()
**Rule**: Escape a string for use as a CSV field.

**Signature**: `string ToCsvField(this string value)`

**Escaping Rules**:
- If contains comma, quote, or newline → Wrap in quotes
- Double quotes inside value → Escaped as `""`
- Already quoted values → Add outer quotes

```csharp
"hello".ToCsvField()                    // "hello"
"hello, world".ToCsvField()             // "\"hello, world\""
"say \"hello\"".ToCsvField()            // "\"say \"\"hello\"\"\""
"line1\nline2".ToCsvField()             // "\"line1\nline2\""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- Preserves internal newlines (wrapped in quotes)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);

bool needsQuotes = value.Contains(',') || value.Contains('"') ||
                   value.Contains('\n') || value.Contains('\r');

if (!needsQuotes) return value;

return "\"" + value.Replace("\"", "\"\"") + "\"";
```

---

### FromCsvField()
**Rule**: Unescape a CSV field (reverse of ToCsvField).

**Signature**: `string FromCsvField(this string value)`

```csharp
"hello".FromCsvField()                          // "hello"
"\"hello, world\"".FromCsvField()               // "hello, world"
"\"say \"\"hello\"\"\"".FromCsvField()          // "say \"hello\""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Unquoted values returned as-is
- Removes outer quotes if present
- Unescapes internal double quotes (`""` → `"`)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);

if (value.StartsWith('"') && value.EndsWith('"') && value.Length >= 2)
{
    return value.Substring(1, value.Length - 2).Replace("\"\"", "\"");
}
return value;
```

---

### SplitCsvLine()
**Rule**: Split a CSV line into fields, respecting quotes and escapes.

**Signature**: `string[] SplitCsvLine(this string value)`

```csharp
"a,b,c".SplitCsvLine()                          // ["a", "b", "c"]
"\"a,b\",c,d".SplitCsvLine()                    // ["a,b", "c", "d"]
"a,\"b\"\"c\"\"\",d".SplitCsvLine()             // ["a", "b\"c\"", "d"]
```

**Parsing Rules**:
- Comma is delimiter (unless inside quotes)
- Quoted fields can contain commas and newlines
- Double quotes inside quoted fields are escaped as `""`

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns single empty field
- Trailing comma adds empty field
- Handles mixed quoted/unquoted fields

---

### ToCsvLine()
**Rule**: Convert array/list to CSV line.

**Signature**: `string ToCsvLine(this string[] values)`

```csharp
new[] { "a", "b", "c" }.ToCsvLine()             // "a,b,c"
new[] { "a,b", "c" }.ToCsvLine()                // "\"a,b\",c"
new[] { "say \"hi\"" }.ToCsvLine()              // "\"say \"\"hi\"\"\""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null array
- Empty array returns empty string
- Null elements treated as empty strings
- Each element automatically escaped via ToCsvField()

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(values);
return string.Join(",", values.Select(v => (v ?? "").ToCsvField()));
```

---

### ToCsv() - 2D Array
**Rule**: Convert 2D array to CSV string.

**Signature**: `string ToCsv(this string[,] data)`

```csharp
var data = new string[,] {
    { "Name", "Age" },
    { "John", "30" },
    { "Jane", "25" }
};
data.ToCsv();
// "Name,Age\nJohn,30\nJane,25"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null data
- Empty array returns empty string
- Null cells treated as empty strings
- Uses Unix line endings (`\n`)

---

### ToCsv() - List of Lists
**Rule**: Convert list of lists to CSV string.

**Signature**: `string ToCsv(this List<List<string>> data)`

```csharp
var data = new List<List<string>> {
    new() { "Name", "Age" },
    new() { "John", "30" }
};
data.ToCsv();
// "Name,Age\nJohn,30"
```

---

### FromCsvToArray()
**Rule**: Parse CSV string to 2D array.

**Signature**: `string[,] FromCsvToArray(this string csv)`

```csharp
"Name,Age\nJohn,30\nJane,25".FromCsvToArray();
// string[3,2] { {"Name","Age"}, {"John","30"}, {"Jane","25"} }
```

---

## JSON Operations

### ToJsonString()
**Rule**: Escape string for use in JSON (not full serialization).

**Signature**: `string ToJsonString(this string value)`

**Escaping Rules**:
- `"` → `\"`
- `\` → `\\`
- `/` → `\/` (optional but safe)
- `\b` → `\\b` (backspace)
- `\f` → `\\f` (form feed)
- `\n` → `\\n` (newline)
- `\r` → `\\r` (carriage return)
- `\t` → `\\t` (tab)

```csharp
"hello".ToJsonString()                          // "hello"
"say \"hello\"".ToJsonString()                  // "say \\\"hello\\\""
"line1\nline2".ToJsonString()                   // "line1\\nline2"
"C:\\path\\file".ToJsonString()                 // "C:\\\\path\\\\file"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Unicode characters passed through (JSON supports Unicode)
- Control characters escaped

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);

return value
    .Replace("\\", "\\\\")  // Must be first!
    .Replace("\"", "\\\"")
    .Replace("\b", "\\b")
    .Replace("\f", "\\f")
    .Replace("\n", "\\n")
    .Replace("\r", "\\r")
    .Replace("\t", "\\t");
```

---

### FromJsonString()
**Rule**: Unescape JSON string (reverse of ToJsonString).

**Signature**: `string FromJsonString(this string value)`

```csharp
"hello".FromJsonString()                        // "hello"
"say \\\"hello\\\"".FromJsonString()            // "say \"hello\""
"line1\\nline2".FromJsonString()                // "line1\nline2"
```

**Implementation**: Reverse of ToJsonString() - replace escape sequences with actual characters.

---

### IsValidJson()
**Rule**: Check if string is valid JSON.

**Signature**: `bool IsValidJson(this string value)`

```csharp
"{\"name\":\"John\"}".IsValidJson()             // true
"[1,2,3]".IsValidJson()                         // true
"\"hello\"".IsValidJson()                       // true
"not json".IsValidJson()                        // false
"{invalid}".IsValidJson()                       // false
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Uses `System.Text.Json` for validation
- Accepts any valid JSON (object, array, string, number, boolean, null)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
try
{
    JsonDocument.Parse(value);
    return true;
}
catch (JsonException)
{
    return false;
}
```

---

### ToJsonArray() - String Array
**Rule**: Convert string array to JSON array string.

**Signature**: `string ToJsonArray(this string[] values)`

```csharp
new[] { "a", "b", "c" }.ToJsonArray()           // "[\"a\",\"b\",\"c\"]"
new[] { "say \"hi\"" }.ToJsonArray()            // "[\"say \\\"hi\\\"\"]"
```

---

### ToJsonArray() - List of Lists
**Rule**: Convert list of lists to JSON 2D array string.

**Signature**: `string ToJsonArray(this List<List<string>> data)`

```csharp
var data = new List<List<string>> {
    new() { "Name", "Age" },
    new() { "John", "30" }
};
data.ToJsonArray();
// "[[\"Name\",\"Age\"],[\"John\",\"30\"]]"
```

---

## XML Operations

### ToXmlContent()
**Rule**: Escape string for use in XML element content.

**Signature**: `string ToXmlContent(this string value)`

**Escaping Rules**:
- `<` → `&lt;`
- `>` → `&gt;`
- `&` → `&amp;`
- `"` → `&quot;`
- `'` → `&apos;`

```csharp
"<tag>".ToXmlContent()                          // "&lt;tag&gt;"
"Tom & Jerry".ToXmlContent()                    // "Tom &amp; Jerry"
"say \"hello\"".ToXmlContent()                  // "say &quot;hello&quot;"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Empty string returns empty string
- `&` must be escaped first to avoid double-escaping

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);

return value
    .Replace("&", "&amp;")   // Must be first!
    .Replace("<", "&lt;")
    .Replace(">", "&gt;")
    .Replace("\"", "&quot;")
    .Replace("'", "&apos;");
```

---

### FromXmlContent()
**Rule**: Unescape XML entities (reverse of ToXmlContent).

**Signature**: `string FromXmlContent(this string value)`

```csharp
"&lt;tag&gt;".FromXmlContent()                  // "<tag>"
"Tom &amp; Jerry".FromXmlContent()              // "Tom & Jerry"
```

**Implementation**: Reverse order - unescape `&amp;` last.

---

## Base64 Operations

### ToBase64()
**Rule**: Encode string to Base64.

**Signature**: `string ToBase64(this string value)`

```csharp
"hello".ToBase64()                              // "aGVsbG8="
"hello world".ToBase64()                        // "aGVsbG8gd29ybGQ="
"".ToBase64()                                   // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Uses UTF-8 encoding
- Empty string returns empty string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (value.Length == 0) return string.Empty;

byte[] bytes = Encoding.UTF8.GetBytes(value);
return Convert.ToBase64String(bytes);
```

---

### FromBase64()
**Rule**: Decode Base64 string.

**Signature**: `string FromBase64(this string value)`

```csharp
"aGVsbG8=".FromBase64()                         // "hello"
"aGVsbG8gd29ybGQ=".FromBase64()                 // "hello world"
"".FromBase64()                                 // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `FormatException` for invalid Base64
- Uses UTF-8 decoding
- Empty string returns empty string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (value.Length == 0) return string.Empty;

byte[] bytes = Convert.FromBase64String(value);
return Encoding.UTF8.GetString(bytes);
```

---

### IsValidBase64()
**Rule**: Check if string is valid Base64.

**Signature**: `bool IsValidBase64(this string value)`

```csharp
"aGVsbG8=".IsValidBase64()                      // true
"not base64!".IsValidBase64()                   // false
"aGVsbG8".IsValidBase64()                       // true (padding optional)
```

**Validation Rules**:
- Characters: `A-Z`, `a-z`, `0-9`, `+`, `/`
- Padding: `=` (optional)
- Length: Multiple of 4 (if padded)

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
try
{
    Convert.FromBase64String(value);
    return true;
}
catch (FormatException)
{
    return false;
}
```

---

## URL Operations

### ToUrlEncoded()
**Rule**: URL-encode string (percent encoding).

**Signature**: `string ToUrlEncoded(this string value)`

```csharp
"hello world".ToUrlEncoded()                    // "hello+world" or "hello%20world"
"hello@example.com".ToUrlEncoded()              // "hello%40example.com"
"100%".ToUrlEncoded()                           // "100%25"
```

**Encoding Rules**:
- Space → `+` or `%20`
- Reserved characters → `%XX` (hex)
- Unreserved: `A-Z a-z 0-9 - _ . ~`

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Uses `Uri.EscapeDataString()` for RFC 3986 compliance
- Empty string returns empty string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return Uri.EscapeDataString(value);
```

---

### FromUrlEncoded()
**Rule**: URL-decode string (reverse of ToUrlEncoded).

**Signature**: `string FromUrlEncoded(this string value)`

```csharp
"hello+world".FromUrlEncoded()                  // "hello world"
"hello%40example.com".FromUrlEncoded()          // "hello@example.com"
"100%25".FromUrlEncoded()                       // "100%"
```

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return Uri.UnescapeDataString(value);
```

---

## HTML Operations

### ToHtmlEncoded()
**Rule**: HTML-encode string for safe display.

**Signature**: `string ToHtmlEncoded(this string value)`

**Escaping Rules**:
- `<` → `&lt;`
- `>` → `&gt;`
- `&` → `&amp;`
- `"` → `&quot;`
- `'` → `&#39;` or `&apos;`

```csharp
"<script>".ToHtmlEncoded()                      // "&lt;script&gt;"
"Tom & Jerry".ToHtmlEncoded()                   // "Tom &amp; Jerry"
"say \"hello\"".ToHtmlEncoded()                 // "say &quot;hello&quot;"
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Uses `System.Net.WebUtility.HtmlEncode()`
- Prevents XSS attacks

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return System.Net.WebUtility.HtmlEncode(value);
```

---

### FromHtmlEncoded()
**Rule**: HTML-decode string (reverse of ToHtmlEncoded).

**Signature**: `string FromHtmlEncoded(this string value)`

```csharp
"&lt;script&gt;".FromHtmlEncoded()              // "<script>"
"Tom &amp; Jerry".FromHtmlEncoded()             // "Tom & Jerry"
```

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return System.Net.WebUtility.HtmlDecode(value);
```

---

## Hex Operations

### ToHex()
**Rule**: Convert string to hexadecimal representation.

**Signature**: `string ToHex(this string value)`

```csharp
"hello".ToHex()                                 // "68656C6C6F"
"A".ToHex()                                     // "41"
"".ToHex()                                      // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Uses UTF-8 encoding
- Uppercase hex digits by default
- Empty string returns empty string

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
byte[] bytes = Encoding.UTF8.GetBytes(value);
return BitConverter.ToString(bytes).Replace("-", "");
```

---

### FromHex()
**Rule**: Convert hexadecimal string to text.

**Signature**: `string FromHex(this string value)`

```csharp
"68656C6C6F".FromHex()                          // "hello"
"41".FromHex()                                  // "A"
"".FromHex()                                    // ""
```

**Edge Cases**:
- Throws `ArgumentNullException` for null value
- Throws `FormatException` for invalid hex
- Expects even number of hex digits
- Case-insensitive

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
if (value.Length % 2 != 0)
    throw new FormatException("Hex string must have even length");

byte[] bytes = new byte[value.Length / 2];
for (int i = 0; i < bytes.Length; i++)
{
    bytes[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
}
return Encoding.UTF8.GetString(bytes);
```

---

### IsValidHex()
**Rule**: Check if string is valid hexadecimal.

**Signature**: `bool IsValidHex(this string value)`

```csharp
"68656C6C6F".IsValidHex()                       // true
"ABCDEF".IsValidHex()                           // true
"XYZ".IsValidHex()                              // false
"123".IsValidHex()                              // false (odd length)
```

**Validation Rules**:
- Characters: `0-9`, `A-F`, `a-f`
- Length: Even number of digits

**Implementation**:
```csharp
ArgumentNullException.ThrowIfNull(value);
return value.Length % 2 == 0 &&
       value.All(c => "0123456789ABCDEFabcdef".Contains(c));
```

---

## Performance Considerations

| Operation | Complexity | Notes |
|-----------|-----------|-------|
| CSV | O(n) | String manipulation |
| JSON | O(n) | String escaping |
| XML | O(n) | Entity replacement |
| Base64 | O(n) | Byte conversion |
| URL | O(n) | Percent encoding |
| HTML | O(n) | Entity encoding |
| Hex | O(n) | Byte-to-hex conversion |

**Optimization**: All methods use built-in .NET encoders where possible for performance and security.

---

This specification ensures consistent data format and encoding behavior!
