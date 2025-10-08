# Casing Rules Reference

Detailed specification for all casing transformations in FluentBuilders.Text.

---

## ToUpper()
**Rule**: Convert all characters to uppercase.

```csharp
"hello world" → "HELLO WORLD"
"Hello World" → "HELLO WORLD"
"hello123"    → "HELLO123"
```

**Implementation**: `string.ToUpperInvariant()`

---

## ToLower()
**Rule**: Convert all characters to lowercase.

```csharp
"HELLO WORLD" → "hello world"
"Hello World" → "hello world"
"HELLO123"    → "hello123"
```

**Implementation**: `string.ToLowerInvariant()`

---

## ToTitleCase()
**Rule**: Capitalize the first letter of **every word**.

```csharp
"hello world"           → "Hello World"
"the quick brown fox"   → "The Quick Brown Fox"
"123 hello world"       → "123 Hello World"
"hello-world test"      → "Hello-World Test"
```

**Word Definition**: Separated by spaces, but preserves hyphens/apostrophes within words.

**Implementation**: `TextInfo.ToTitleCase()` or custom for better control.

---

## ToSentenceCase()
**Rule**: Capitalize **only the first letter** of the entire string.

```csharp
"hello world"           → "Hello world"
"the quick brown fox"   → "The quick brown fox"
"HELLO WORLD"           → "Hello world"
"hello. world"          → "Hello. world"  (only first letter, not after period)
```

**Implementation**:
```csharp
if (string.IsNullOrEmpty(value)) return value;
return char.ToUpperInvariant(value[0]) + value[1..].ToLowerInvariant();
```

---

## ToProperCase()
**Rule**: Alias for `ToTitleCase()`. Commonly used in business contexts.

```csharp
"hello world" → "Hello World"
```

**Implementation**: Delegates to `ToTitleCase()`.

---

## ToNameCase()
**Rule**: Smart capitalization for **human names**, handling special cases.

### Rules:
1. Capitalize first letter of each word (space-separated)
2. Preserve capitalization after apostrophes: `O'Brien`, `D'Angelo`
3. Preserve capitalization after hyphens: `Mary-Jane`, `Jean-Claude`
4. Handle name prefixes: `McDonald`, `MacArthur`, `von Neumann`
5. Handle Roman numerals: `Henry VIII`, `Louis XIV`

```csharp
"john smith"            → "John Smith"
"john o'brien"          → "John O'Brien"
"mary-jane watson"      → "Mary-Jane Watson"
"jean-claude van damme" → "Jean-Claude Van Damme"
"mcdonald"              → "McDonald"
"macarthur"             → "MacArthur"
"henry viii"            → "Henry VIII"
"de la cruz"            → "de la Cruz"  (lowercase particles)
"von neumann"           → "von Neumann" (lowercase particles)
```

**Special Handling**:
- `Mc*` → `Mc` + uppercase next letter: `mcdonald` → `McDonald`
- `Mac*` → `Mac` + uppercase next letter: `macarthur` → `MacArthur`
- `O'*` → `O'` + uppercase next letter: `o'brien` → `O'Brien`
- `D'*` → `D'` + uppercase next letter: `d'angelo` → `D'Angelo`
- Roman numerals: `I, II, III, IV, V, VI, VII, VIII, IX, X, XI, XII, XIII, XIV, XV, XVI, XVII, XVIII, XIX, XX`
- Name particles (lowercase): `de`, `la`, `von`, `van`, `der`, `den`, `dos`, `das`, `el`

**Implementation**: Custom algorithm with pattern matching.

---

## ToPascalCase()
**Rule**: Capitalize first letter of each word, **remove spaces**.

```csharp
"hello world"       → "HelloWorld"
"the quick fox"     → "TheQuickFox"
"hello_world"       → "HelloWorld"
"hello-world"       → "HelloWorld"
"hello.world"       → "HelloWorld"
"hello123world"     → "Hello123World"
```

**Word Boundaries**: Space, underscore, hyphen, dot, digit-to-letter transition.

**Implementation**:
```csharp
Split by: [ _-.]
Capitalize each part
Join without separator
```

---

## ToCamelCase()
**Rule**: Like PascalCase, but **first letter lowercase**.

```csharp
"hello world"       → "helloWorld"
"the quick fox"     → "theQuickFox"
"hello_world"       → "helloWorld"
"HelloWorld"        → "helloWorld"
```

**Implementation**: `ToPascalCase()` then lowercase first character.

---

## ToKebabCase()
**Rule**: Lowercase, words separated by **hyphens**.

```csharp
"hello world"       → "hello-world"
"HelloWorld"        → "hello-world"
"helloWorld"        → "hello-world"
"hello_world"       → "hello-world"
"Hello World 123"   → "hello-world-123"
```

**Implementation**:
```csharp
Insert hyphen before uppercase letters (except first)
Replace spaces/underscores with hyphens
Lowercase all
```

---

## ToSnakeCase()
**Rule**: Lowercase, words separated by **underscores**.

```csharp
"hello world"       → "hello_world"
"HelloWorld"        → "hello_world"
"helloWorld"        → "hello_world"
"hello-world"       → "hello_world"
"Hello World 123"   → "hello_world_123"
```

**Implementation**:
```csharp
Insert underscore before uppercase letters (except first)
Replace spaces/hyphens with underscores
Lowercase all
```

---

## ToUpperFirst()
**Rule**: Uppercase **only the first character**, leave rest unchanged.

```csharp
"hello"     → "Hello"
"hELLO"     → "HELLO"
"123hello"  → "123hello"  (no change, starts with digit)
```

**Implementation**:
```csharp
if (char.IsLetter(value[0]))
    return char.ToUpperInvariant(value[0]) + value[1..];
```

---

## ToLowerFirst()
**Rule**: Lowercase **only the first character**, leave rest unchanged.

```csharp
"Hello"     → "hello"
"HELLO"     → "hELLO"
"123Hello"  → "123Hello"  (no change, starts with digit)
```

**Implementation**:
```csharp
if (char.IsLetter(value[0]))
    return char.ToLowerInvariant(value[0]) + value[1..];
```

---

## Edge Cases

### Empty/Null Strings
All methods should:
- Return `string.Empty` for empty input
- Throw `ArgumentNullException` for null input (or provide `OrEmpty()` variants)

### Unicode
Use `InvariantCulture` for predictable behavior:
```csharp
"ñoño".ToUpperInvariant()  // "ÑOÑO"
"café".ToTitleCase()       // "Café"
```

### Numbers & Symbols
```csharp
"123hello"        → "123hello"     (ToSentenceCase)
"hello123world"   → "Hello123world" (ToSentenceCase)
"!hello"          → "!hello"       (ToSentenceCase, skip non-letter)
```

### Multiple Spaces
```csharp
"hello  world"    → "Hello  World"  (ToTitleCase preserves spacing)
"hello  world"    → "Hello  world"  (ToSentenceCase preserves spacing)
```

---

## Test Matrix

Each casing method should be tested with:

1. **Basic**: `"hello"`, `"HELLO"`, `"Hello"`
2. **Multi-word**: `"hello world"`, `"the quick brown fox"`
3. **Mixed case**: `"hElLo WoRlD"`
4. **Numbers**: `"hello123"`, `"123hello"`, `"hello123world"`
5. **Symbols**: `"hello-world"`, `"hello_world"`, `"hello.world"`
6. **Unicode**: `"café"`, `"ñoño"`, `"Ü"`
7. **Edge cases**: `""`, single char, all spaces
8. **Names (ToNameCase)**:
   - `"john o'brien"` → `"John O'Brien"`
   - `"mary-jane"` → `"Mary-Jane"`
   - `"mcdonald"` → `"McDonald"`
   - `"henry viii"` → `"Henry VIII"`
   - `"von neumann"` → `"von Neumann"`

---

## Performance Considerations

- Use `Span<char>` for allocation-free transformations where possible
- Cache `TextInfo.ToTitleCase()` instance
- Avoid regex for simple operations
- Benchmark against BCL methods

---

This specification ensures consistent, predictable casing behavior across all methods!
