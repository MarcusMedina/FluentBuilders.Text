namespace MarcusMedina.Fluent.Text.Extensions.DataFormat;

using System.Net;
using System.Text;
using System.Text.Json;

/// <summary>
/// Extension methods for working with data formats like CSV, JSON, and XML.
/// Useful for teaching data formats and escaping rules.
/// </summary>
public static class StringDataFormatExtensions
{
    #region Private Fields

    // Maximum byte array size for decoding operations (100MB)
    private const long MaxByteArraySize = 100_000_000;

    // Maximum result length: 100MB of chars (200MB memory as char = 2 bytes)
    private const long MaxResultLength = 50_000_000;

    #endregion Private Fields

    #region CSV Operations

    /// <summary>
    /// Parses a CSV field, removing quotes and unescaping internal quotes.
    /// </summary>
    /// <param name="value">The CSV field to parse.</param>
    /// <returns>The unescaped value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".FromCsvField()                  // "hello"
    /// "\"hello, world\"".FromCsvField()       // "hello, world"
    /// "\"say \"\"hello\"\"\"".FromCsvField()  // "say \"hello\""
    /// </code>
    /// </example>
    public static string FromCsvField(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        // Remove surrounding quotes if present
        if (value.Length >= 2 && value[0] == '"' && value[^1] == '"')
        {
            value = value[1..^1];
        }

        // Unescape doubled quotes
        return value.Replace("\"\"", "\"");
    }

    /// <summary>
    /// Splits a CSV line into fields, respecting quoted values.
    /// </summary>
    /// <param name="value">The CSV line to split.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>Array of parsed fields.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "a,b,c".SplitCsvLine()                          // ["a", "b", "c"]
    /// "a;b;c".SplitCsvLine(';')                       // ["a", "b", "c"]
    /// "\"hello, world\",test".SplitCsvLine()          // ["hello, world", "test"]
    /// "\"say \"\"hi\"\"\",data".SplitCsvLine()        // ["say \"hi\"", "data"]
    /// </code>
    /// </example>
    public static string[] SplitCsvLine(this string value, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(value);

        var fields = new List<string>();
        var currentField = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];

            if (c == '"')
            {
                // Check for escaped quote
                if (inQuotes && i + 1 < value.Length && value[i + 1] == '"')
                {
                    currentField.Append('"');
                    i++; // Skip next quote
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == delimiter && !inQuotes)
            {
                fields.Add(currentField.ToString());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        fields.Add(currentField.ToString());
        return [.. fields];
    }

    /// <summary>
    /// Escapes a string for use in CSV format.
    /// Handles commas, semicolons, quotes, and newlines by wrapping in quotes and escaping internal quotes.
    /// </summary>
    /// <param name="value">The string to escape.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>The CSV-escaped string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".ToCsvField()                    // "hello"
    /// "hello, world".ToCsvField()             // "\"hello, world\""
    /// "say \"hello\"".ToCsvField()            // "\"say \"\"hello\"\"\""
    /// "value;data".ToCsvField(';')            // "\"value;data\""
    /// </code>
    /// </example>
    public static string ToCsvField(this string value, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(value);

        // Check if escaping is needed
        bool needsQuotes = value.Contains(delimiter) ||
                          value.Contains('"') ||
                          value.Contains('\n') ||
                          value.Contains('\r');

        if (!needsQuotes)
        {
            return value;
        }

        // Escape quotes by doubling them
        var escaped = value.Replace("\"", "\"\"");

        // Wrap in quotes
        return $"\"{escaped}\"";
    }

    /// <summary>
    /// Joins values into a CSV line, automatically escaping fields as needed.
    /// </summary>
    /// <param name="values">The values to join.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>A properly formatted CSV line.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is null.</exception>
    /// <example>
    /// <code>
    /// new[] { "a", "b", "c" }.ToCsvLine()                     // "a,b,c"
    /// new[] { "a", "b", "c" }.ToCsvLine(';')                  // "a;b;c"
    /// new[] { "hello, world", "test" }.ToCsvLine()            // "\"hello, world\",test"
    /// </code>
    /// </example>
    public static string ToCsvLine(this IEnumerable<string> values, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(values);

        return string.Join(delimiter.ToString(), values.Select(v => v.ToCsvField(delimiter)));
    }

    #endregion CSV Operations

    #region JSON Operations

    /// <summary>
    /// Parses CSV string into a 2D array.
    /// </summary>
    /// <param name="value">The CSV string to parse.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>2D array of parsed values.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Name,Age\nJohn,30".FromCsvToArray();
    /// // string[2,2] { { "Name", "Age" }, { "John", "30" } }
    /// </code>
    /// </example>
    public static string[,] FromCsvToArray(this string value, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return new string[0, 0];
        }

        var lines = value.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
        if (lines.Length == 0)
        {
            return new string[0, 0];
        }

        var rows = lines.Select(line => line.SplitCsvLine(delimiter)).ToArray();
        int maxCols = rows.Max(r => r.Length);

        var result = new string[rows.Length, maxCols];
        for (int i = 0; i < rows.Length; i++)
        {
            for (int j = 0; j < rows[i].Length; j++)
            {
                result[i, j] = rows[i][j];
            }
        }

        return result;
    }

    /// <summary>
    /// Parses CSV string into a list of rows.
    /// </summary>
    /// <param name="value">The CSV string to parse.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>List of rows (each row is a list of strings).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "Name,Age\nJohn,30".FromCsvToList();
    /// // List&lt;List&lt;string&gt;&gt; { { "Name", "Age" }, { "John", "30" } }
    /// </code>
    /// </example>
    public static List<List<string>> FromCsvToList(this string value, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        var lines = value.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l));
        return lines.Select(line => line.SplitCsvLine(delimiter).ToList()).ToList();
    }

    /// <summary>
    /// Unescapes a JSON string (removes escape sequences).
    /// </summary>
    /// <param name="value">The JSON string to unescape.</param>
    /// <returns>The unescaped string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".FromJsonString()                // "hello"
    /// "say \\\"hi\\\"".FromJsonString()       // "say \"hi\""
    /// "line1\\nline2".FromJsonString()        // "line1\nline2"
    /// </code>
    /// </example>
    public static string FromJsonString(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var sb = new StringBuilder(value.Length);
        bool escape = false;

        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];

            if (escape)
            {
                switch (c)
                {
                    case '"':
                        sb.Append('"');
                        break;

                    case '\\':
                        sb.Append('\\');
                        break;

                    case '/':
                        sb.Append('/');
                        break;

                    case 'b':
                        sb.Append('\b');
                        break;

                    case 'f':
                        sb.Append('\f');
                        break;

                    case 'n':
                        sb.Append('\n');
                        break;

                    case 'r':
                        sb.Append('\r');
                        break;

                    case 't':
                        sb.Append('\t');
                        break;

                    case 'u':
                        // Unicode escape sequence
                        if (i + 4 < value.Length)
                        {
                            var hex = value.Substring(i + 1, 4);
                            if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int unicode))
                            {
                                sb.Append((char)unicode);
                                i += 4;
                            }
                        }

                        break;

                    default:
                        sb.Append(c);
                        break;
                }

                escape = false;
            }
            else if (c == '\\')
            {
                escape = true;
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Parses JSON array into a 2D array.
    /// </summary>
    /// <param name="value">The JSON string to parse (must be array of arrays).</param>
    /// <returns>2D array of parsed values.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "[["Name","Age"],["John","30"]]".FromJsonToArray();
    /// // string[2,2] { { "Name", "Age" }, { "John", "30" } }
    /// </code>
    /// </example>
    public static string[,] FromJsonToArray(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return new string[0, 0];
        }

        using var document = JsonDocument.Parse(value);
        var root = document.RootElement;

        if (root.ValueKind != JsonValueKind.Array)
        {
            return new string[0, 0];
        }

        var rows = new List<List<string>>();
        foreach (var row in root.EnumerateArray())
        {
            var cols = new List<string>();
            foreach (var cell in row.EnumerateArray())
            {
                cols.Add(cell.GetString() ?? string.Empty);
            }

            rows.Add(cols);
        }

        if (rows.Count == 0)
        {
            return new string[0, 0];
        }

        int maxCols = rows.Max(r => r.Count);
        var result = new string[rows.Count, maxCols];

        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Count; j++)
            {
                result[i, j] = rows[i][j];
            }
        }

        return result;
    }

    /// <summary>
    /// Parses JSON array into a list of rows.
    /// </summary>
    /// <param name="value">The JSON string to parse (must be array of arrays).</param>
    /// <returns>List of rows (each row is a list of strings).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "[["Name","Age"],["John","30"]]".FromJsonToList();
    /// // List&lt;List&lt;string&gt;&gt; { { "Name", "Age" }, { "John", "30" } }
    /// </code>
    /// </example>
    public static List<List<string>> FromJsonToList(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        using var document = JsonDocument.Parse(value);
        var root = document.RootElement;

        if (root.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var result = new List<List<string>>();
        foreach (var row in root.EnumerateArray())
        {
            var cols = new List<string>();
            foreach (var cell in row.EnumerateArray())
            {
                cols.Add(cell.GetString() ?? string.Empty);
            }

            result.Add(cols);
        }

        return result;
    }

    /// <summary>
    /// Validates if a string is valid JSON.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>True if the string is valid JSON; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "{\"name\":\"John\"}".IsValidJson()     // true
    /// "not json".IsValidJson()                // false
    /// "[]".IsValidJson()                      // true
    /// </code>
    /// </example>
    public static bool IsValidJson(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(value);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// Converts a 2D array to CSV format.
    /// </summary>
    /// <param name="data">The 2D array to convert.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>CSV formatted string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    /// <example>
    /// <code>
    /// var data = new string[,] { { "Name", "Age" }, { "John", "30" } };
    /// data.ToCsv();  // "Name,Age\nJohn,30"
    /// </code>
    /// </example>
    public static string ToCsv(this string[,] data, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(data);

        var rows = new List<string>();
        for (int i = 0; i < data.GetLength(0); i++)
        {
            var row = new List<string>();
            for (int j = 0; j < data.GetLength(1); j++)
            {
                row.Add(data[i, j] ?? string.Empty);
            }

            rows.Add(row.ToCsvLine(delimiter));
        }

        return string.Join("\n", rows);
    }

    /// <summary>
    /// Converts a list of rows to CSV format.
    /// </summary>
    /// <param name="data">The list of rows to convert.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>CSV formatted string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    /// <example>
    /// <code>
    /// var data = new List&lt;List&lt;string&gt;&gt; {
    ///     new() { "Name", "Age" },
    ///     new() { "John", "30" }
    /// };
    /// data.ToCsv();  // "Name,Age\nJohn,30"
    /// </code>
    /// </example>
    public static string ToCsv(this List<List<string>> data, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(data);

        var rows = data.Select(row => row.ToCsvLine(delimiter));
        return string.Join("\n", rows);
    }

    /// <summary>
    /// Converts an enumerable of rows to CSV format.
    /// </summary>
    /// <param name="data">The enumerable of rows to convert.</param>
    /// <param name="delimiter">The CSV delimiter (default: comma).</param>
    /// <returns>CSV formatted string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    /// <example>
    /// <code>
    /// IEnumerable&lt;IEnumerable&lt;string&gt;&gt; data = new[] {
    ///     new[] { "Name", "Age" },
    ///     new[] { "John", "30" }
    /// };
    /// data.ToCsv();  // "Name,Age\nJohn,30"
    /// </code>
    /// </example>
    public static string ToCsv(this IEnumerable<IEnumerable<string>> data, char delimiter = ',')
    {
        ArgumentNullException.ThrowIfNull(data);

        var rows = data.Select(row => row.ToCsvLine(delimiter));
        return string.Join("\n", rows);
    }

    /// <summary>
    /// Converts a 2D array to JSON format.
    /// </summary>
    /// <param name="data">The 2D array to convert.</param>
    /// <returns>JSON formatted string (array of arrays).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    /// <example>
    /// <code>
    /// var data = new string[,] { { "Name", "Age" }, { "John", "30" } };
    /// data.ToJsonArray();  // [["Name","Age"],["John","30"]]
    /// </code>
    /// </example>
    public static string ToJsonArray(this string[,] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var json = new StringBuilder("[");
        for (int i = 0; i < data.GetLength(0); i++)
        {
            if (i > 0)
            {
                json.Append(',');
            }

            json.Append('[');
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (j > 0)
                {
                    json.Append(',');
                }

                json.Append('"').Append((data[i, j] ?? string.Empty).ToJsonString()).Append('"');
            }

            json.Append(']');
        }

        json.Append(']');
        return json.ToString();
    }

    /// <summary>
    /// Converts a list of rows to JSON format.
    /// </summary>
    /// <param name="data">The list of rows to convert.</param>
    /// <returns>JSON formatted string (array of arrays).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    /// <example>
    /// <code>
    /// var data = new List&lt;List&lt;string&gt;&gt; {
    ///     new() { "Name", "Age" },
    ///     new() { "John", "30" }
    /// };
    /// data.ToJsonArray();  // [["Name","Age"],["John","30"]]
    /// </code>
    /// </example>
    public static string ToJsonArray(this List<List<string>> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var json = new StringBuilder("[");
        for (int i = 0; i < data.Count; i++)
        {
            if (i > 0)
            {
                json.Append(',');
            }

            json.Append('[');
            for (int j = 0; j < data[i].Count; j++)
            {
                if (j > 0)
                {
                    json.Append(',');
                }

                json.Append('"').Append(data[i][j].ToJsonString()).Append('"');
            }

            json.Append(']');
        }

        json.Append(']');
        return json.ToString();
    }

    /// <summary>
    /// Converts an enumerable of rows to JSON format.
    /// </summary>
    /// <param name="data">The enumerable of rows to convert.</param>
    /// <returns>JSON formatted string (array of arrays).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null.</exception>
    /// <example>
    /// <code>
    /// IEnumerable&lt;IEnumerable&lt;string&gt;&gt; data = new[] {
    ///     new[] { "Name", "Age" },
    ///     new[] { "John", "30" }
    /// };
    /// data.ToJsonArray();  // [["Name","Age"],["John","30"]]
    /// </code>
    /// </example>
    public static string ToJsonArray(this IEnumerable<IEnumerable<string>> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var json = new StringBuilder("[");
        bool firstRow = true;
        foreach (var row in data)
        {
            if (!firstRow)
            {
                json.Append(',');
            }

            firstRow = false;

            json.Append('[');
            bool firstCol = true;
            foreach (var cell in row)
            {
                if (!firstCol)
                {
                    json.Append(',');
                }

                firstCol = false;
                json.Append('"').Append(cell.ToJsonString()).Append('"');
            }

            json.Append(']');
        }

        json.Append(']');
        return json.ToString();
    }

    /// <summary>
    /// Escapes a string for use in JSON (handles quotes, backslashes, newlines, tabs, etc.).
    /// </summary>
    /// <param name="value">The string to escape.</param>
    /// <returns>The JSON-escaped string (without surrounding quotes).</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".ToJsonString()          // "hello"
    /// "say \"hi\"".ToJsonString()     // "say \\\"hi\\\""
    /// "line1\nline2".ToJsonString()   // "line1\\nline2"
    /// "path\\file".ToJsonString()     // "path\\\\file"
    /// </code>
    /// </example>
    public static string ToJsonString(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var sb = new StringBuilder(value.Length);

        foreach (char c in value)
        {
            switch (c)
            {
                case '"':
                    sb.Append("\\\"");
                    break;

                case '\\':
                    sb.Append("\\\\");
                    break;

                case '/':
                    sb.Append("\\/");
                    break;

                case '\b':
                    sb.Append("\\b");
                    break;

                case '\f':
                    sb.Append("\\f");
                    break;

                case '\n':
                    sb.Append("\\n");
                    break;

                case '\r':
                    sb.Append("\\r");
                    break;

                case '\t':
                    sb.Append("\\t");
                    break;

                default:
                    if (char.IsControl(c))
                    {
                        sb.Append($"\\u{(int)c:x4}");
                    }
                    else
                    {
                        sb.Append(c);
                    }

                    break;
            }
        }

        return sb.ToString();
    }

    #endregion JSON Operations

    #region XML Operations

    /// <summary>
    /// Unescapes XML entities back to their original characters.
    /// </summary>
    /// <param name="value">The XML string to unescape.</param>
    /// <returns>The unescaped string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".FromXmlContent()                        // "hello"
    /// "&amp;lt;tag&amp;gt;".FromXmlContent()          // "&lt;tag&gt;"
    /// "Tom &amp;amp; Jerry".FromXmlContent()          // "Tom &amp; Jerry"
    /// </code>
    /// </example>
    public static string FromXmlContent(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&quot;", "\"")
            .Replace("&apos;", "'")
            .Replace("&amp;", "&"); // Must be last
    }

    /// <summary>
    /// Escapes a string for use in XML content (handles &lt;, &gt;, &amp;, &quot;, &apos;).
    /// </summary>
    /// <param name="value">The string to escape.</param>
    /// <returns>The XML-escaped string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".ToXmlContent()              // "hello"
    /// "&lt;tag&gt;".ToXmlContent()        // "&amp;lt;tag&amp;gt;"
    /// "Tom &amp; Jerry".ToXmlContent()    // "Tom &amp;amp; Jerry"
    /// </code>
    /// </example>
    public static string ToXmlContent(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
    }

    #endregion XML Operations

    #region Base64 Operations

    /// <summary>
    /// Decodes a Base64 encoded string.
    /// </summary>
    /// <param name="value">The Base64 string to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the string is not valid Base64.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when decoded result would exceed memory limits.</exception>
    /// <example>
    /// <code>
    /// "aGVsbG8gd29ybGQ=".FromBase64()  // "hello world"
    /// "dGVzdA==".FromBase64()          // "test"
    /// </code>
    /// </example>
    public static string FromBase64(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        // Estimate decoded size (base64 is ~4/3 of original, so decoded is ~3/4 of input)
        long estimatedSize = value.Length * 3L / 4L;
        if (estimatedSize > MaxByteArraySize)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"Base64 string too large: would decode to approximately {estimatedSize} bytes, exceeding maximum of {MaxByteArraySize}");
        }

        var bytes = Convert.FromBase64String(value);

        return bytes.Length > MaxByteArraySize
            ? throw new ArgumentOutOfRangeException(nameof(value),
                $"Decoded Base64 resulted in {bytes.Length} bytes, exceeding maximum of {MaxByteArraySize}")
            : Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Checks if a string is valid Base64.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>True if valid Base64; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "aGVsbG8gd29ybGQ=".IsValidBase64()  // true
    /// "not-base64!".IsValidBase64()       // false
    /// </code>
    /// </example>
    public static bool IsValidBase64(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        try
        {
            Convert.FromBase64String(value);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    /// <summary>
    /// Encodes a string to Base64 format.
    /// </summary>
    /// <param name="value">The string to encode.</param>
    /// <returns>Base64 encoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToBase64()  // "aGVsbG8gd29ybGQ="
    /// "test".ToBase64()         // "dGVzdA=="
    /// </code>
    /// </example>
    public static string ToBase64(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    #endregion Base64 Operations

    #region URL Encoding Operations

    /// <summary>
    /// Decodes a URL encoded string.
    /// </summary>
    /// <param name="value">The URL encoded string to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello+world".FromUrlEncoded()       // "hello world"
    /// "a%26b%3Dc".FromUrlEncoded()         // "a&amp;b=c"
    /// "email%40test.com".FromUrlEncoded()  // "email@test.com"
    /// </code>
    /// </example>
    public static string FromUrlEncoded(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return WebUtility.UrlDecode(value);
    }

    /// <summary>
    /// Encodes a string for use in URLs.
    /// </summary>
    /// <param name="value">The string to encode.</param>
    /// <returns>URL encoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello world".ToUrlEncoded()       // "hello+world"
    /// "a&amp;b=c".ToUrlEncoded()         // "a%26b%3Dc"
    /// "email@test.com".ToUrlEncoded()    // "email%40test.com"
    /// </code>
    /// </example>
    public static string ToUrlEncoded(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return WebUtility.UrlEncode(value);
    }

    #endregion URL Encoding Operations

    #region HTML Encoding Operations

    /// <summary>
    /// Decodes an HTML encoded string.
    /// </summary>
    /// <param name="value">The HTML encoded string to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "&amp;lt;script&amp;gt;".FromHtmlEncoded()  // "&lt;script&gt;"
    /// "Tom &amp;amp; Jerry".FromHtmlEncoded()     // "Tom &amp; Jerry"
    /// "&amp;copy;2024".FromHtmlEncoded()          // "©2024"
    /// </code>
    /// </example>
    public static string FromHtmlEncoded(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return WebUtility.HtmlDecode(value);
    }

    /// <summary>
    /// Encodes a string for use in HTML content.
    /// </summary>
    /// <param name="value">The string to encode.</param>
    /// <returns>HTML encoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "&lt;script&gt;".ToHtmlEncoded()       // "&amp;lt;script&amp;gt;"
    /// "Tom &amp; Jerry".ToHtmlEncoded()      // "Tom &amp;amp; Jerry"
    /// "©2024".ToHtmlEncoded()                // "©2024" (preserves Unicode)
    /// </code>
    /// </example>
    public static string ToHtmlEncoded(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return WebUtility.HtmlEncode(value);
    }

    #endregion HTML Encoding Operations

    #region Hex Encoding Operations

    /// <summary>
    /// Converts a hexadecimal string back to its original string value.
    /// </summary>
    /// <param name="value">The hexadecimal string to convert.</param>
    /// <returns>The decoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the string is not valid hex.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when decoded result would exceed memory limits.</exception>
    /// <example>
    /// <code>
    /// "68656C6C6F".FromHex()    // "hello"
    /// "74657374".FromHex()      // "test"
    /// </code>
    /// </example>
    public static string FromHex(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length % 2 != 0)
        {
            throw new FormatException("Hex string must have an even number of characters.");
        }

        long byteCount = value.Length / 2;
        if (byteCount > MaxByteArraySize)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"Hex string too large: would decode to {byteCount} bytes, exceeding maximum of {MaxByteArraySize}");
        }

        var bytes = new byte[byteCount];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
        }

        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Checks if a string is valid hexadecimal.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>True if valid hex; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "68656C6C6F".IsValidHex()    // true
    /// "ABCDEF".IsValidHex()        // true
    /// "GHIJKL".IsValidHex()        // false
    /// "123".IsValidHex()           // false (odd length)
    /// </code>
    /// </example>
    public static bool IsValidHex(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return !string.IsNullOrWhiteSpace(value) && value.Length % 2 == 0 && value.All(c => c is (>= '0' and <= '9') or
                             (>= 'A' and <= 'F') or
                             (>= 'a' and <= 'f'));
    }

    /// <summary>
    /// Converts a string to hexadecimal representation.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="uppercase">Whether to use uppercase hex digits (default: true).</param>
    /// <returns>Hexadecimal string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <example>
    /// <code>
    /// "hello".ToHex()           // "68656C6C6F"
    /// "hello".ToHex(false)      // "68656c6c6f"
    /// "test".ToHex()            // "74657374"
    /// </code>
    /// </example>
    public static string ToHex(this string value, bool uppercase = true)
    {
        ArgumentNullException.ThrowIfNull(value);

        var bytes = Encoding.UTF8.GetBytes(value);
        var format = uppercase ? "X2" : "x2";
        return string.Concat(bytes.Select(b => b.ToString(format)));
    }

    #endregion Hex Encoding Operations
}