namespace Yake.NET;

/// <summary>
/// YAKE — Yet Another Keyword Extractor.
///
/// Unsupervised, lightweight, language-agnostic keyword extraction
/// for single documents. Lower score = more relevant keyword.
///
/// <example>
/// <code>
/// var extractor = new YakeExtractor();
/// var keywords = extractor.Extract("Your document text here...");
///
/// foreach (var kw in keywords)
///     Console.WriteLine(kw); // e.g. "neural network (score: 0.001234)"
/// </code>
/// </example>
/// </summary>
public sealed class YakeExtractor
{
    private readonly YakeOptions _options;
    private readonly HashSet<string> _stopwords;

    /// <summary>Creates a YakeExtractor with default options (English stopwords).</summary>
    public YakeExtractor() : this(new YakeOptions()) { }

    /// <summary>Creates a YakeExtractor with the supplied options.</summary>
    public YakeExtractor(YakeOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        _stopwords = options.Stopwords is not null
            ? new HashSet<string>(options.Stopwords, StringComparer.OrdinalIgnoreCase)
            : StopwordsEn.Words;
    }

    /// <summary>
    /// Extracts keywords from <paramref name="text"/> and returns them ordered
    /// by relevance (lowest YAKE score first).
    /// </summary>
    /// <param name="text">The input document text.</param>
    /// <returns>
    /// A list of <see cref="KeywordResult"/> sorted by score ascending
    /// (most relevant first), capped at <see cref="YakeOptions.TopN"/>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when text is null or whitespace.</exception>
    public IReadOnlyList<KeywordResult> Extract(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Input text must not be null or whitespace.", nameof(text));

        // 1. Tokenise into sentences
        var sentences = TextPreprocessor.Tokenize(text);

        if (sentences.Count == 0)
            return [];

        // 2. Compute per-word features
        var wordFeatures = WordFeaturesComputer.Compute(
            sentences,
            _stopwords,
            _options.WindowSize);

        // 3. Generate and score n-gram candidates
        var scored = CandidateScorer.Score(
            sentences,
            wordFeatures,
            _stopwords,
            _options.MaxNGramSize);

        // 4. Deduplicate near-duplicate candidates
        var deduplicated = Deduplicator.Deduplicate(
            scored,
            _options.DeduplicationThreshold);

        // 5. Return top-N
        return deduplicated
            .Take(_options.TopN)
            .Select(x => new KeywordResult(x.Candidate, x.Score))
            .ToList();
    }
}
