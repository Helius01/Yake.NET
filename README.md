# Yake.NET

A pure C# implementation of **YAKE** (Yet Another Keyword Extractor) — an unsupervised,
lightweight, and language-agnostic keyword extraction algorithm for single documents.

> **Lower score = more relevant keyword.**

---

## Features

- ✅ No external dependencies — 100% pure C#
- ✅ Unsupervised — no training data or models needed
- ✅ Language-agnostic — works with any language when paired with custom stopwords
- ✅ N-gram support — extracts single words and multi-word keyphrases
- ✅ Built-in English stopwords
- ✅ Deduplication of near-duplicate candidates
- ✅ Fully configurable via `YakeOptions`
- ✅ .NET 10

---

## Installation

```bash
dotnet add package Yake.NET
```

---

## Quick Start

```csharp
using Yake.NET;

var extractor = new YakeExtractor();
var keywords = extractor.Extract("Your document text goes here...");

foreach (var kw in keywords)
    Console.WriteLine(kw); // e.g. "keyword extraction (score: 0.001234)"
```

---

## Configuration

```csharp
var extractor = new YakeExtractor(new YakeOptions
{
    MaxNGramSize          = 3,    // include up to trigrams
    TopN                  = 10,   // return top 10 keywords
    DeduplicationThreshold = 0.9, // similarity threshold for dedup (0–1)
    WindowSize            = 2,    // co-occurrence window
    Stopwords             = null  // null = use built-in English list
});
```

### Custom Stopwords

```csharp
// Extend the built-in English stopwords
var customStopwords = StopwordsEn.Words.Concat(["custom", "words"]);

var extractor = new YakeExtractor(new YakeOptions
{
    Stopwords = customStopwords
});
```

---

## How YAKE Works

YAKE scores each candidate keyword using **5 statistical features**:

| Feature | Description |
|---|---|
| **T_Casing** | Ratio of capitalised (non-sentence-start) occurrences |
| **T_Position** | Inverse log of the sentence where the word first appears |
| **T_Frequency** | Normalised term frequency |
| **T_Relatedness** | Diversity of co-occurring words relative to frequency |
| **T_DifferentSentence** | Fraction of sentences containing the word |

The final score formula for n-grams:

```
score(candidate) = ∏(word_scores) / (n × (n + Σ word_scores))
```

---

## API Reference

### `YakeExtractor`

| Member | Description |
|---|---|
| `YakeExtractor()` | Creates extractor with default English options |
| `YakeExtractor(YakeOptions)` | Creates extractor with custom options |
| `IReadOnlyList<KeywordResult> Extract(string text)` | Extracts keywords from text |

### `KeywordResult`

| Property | Type | Description |
|---|---|---|
| `Keyword` | `string` | The extracted keyword or keyphrase |
| `Score` | `double` | YAKE score (lower = more relevant) |

### `YakeOptions`

| Property | Default | Description |
|---|---|---|
| `MaxNGramSize` | `3` | Maximum n-gram length |
| `TopN` | `10` | Number of keywords to return |
| `DeduplicationThreshold` | `0.9` | Similarity cutoff for deduplication |
| `WindowSize` | `2` | Co-occurrence context window size |
| `Stopwords` | `null` | Custom stopwords (null = English built-ins) |

---

## Publishing to NuGet

### Step 1 — Update metadata in `Yake.NET.csproj`

Edit the following fields:

```xml
<Authors>YourName</Authors>
<PackageProjectUrl>https://github.com/YourUsername/Yake.NET</PackageProjectUrl>
<RepositoryUrl>https://github.com/YourUsername/Yake.NET</RepositoryUrl>
<Copyright>Copyright © 2025 YourName</Copyright>
```

### Step 2 — Build in Release mode

```bash
dotnet build src/Yake.NET/Yake.NET.csproj -c Release
```

### Step 3 — Pack

```bash
dotnet pack src/Yake.NET/Yake.NET.csproj -c Release -o ./nupkg
```

This produces `./nupkg/Yake.NET.1.0.0.nupkg`.

### Step 4 — Get a NuGet API key

1. Go to [https://www.nuget.org](https://www.nuget.org) and sign in
2. Go to **Account → API Keys → Create**
3. Set a name, set **Glob pattern** to `Yake.NET`
4. Copy the key

### Step 5 — Push

```bash
dotnet nuget push ./nupkg/Yake.NET.1.0.0.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

Your package will be live on NuGet within a few minutes! 🎉

---

## References

- Campos, R., Mangaravite, V., Pasquali, A., Jorge, A., Nunes, C., & Jatowt, A. (2020).
  *YAKE! Keyword extraction from single documents using multiple local features.*
  Information Sciences, 509, 257–289.

---

## License

MIT
