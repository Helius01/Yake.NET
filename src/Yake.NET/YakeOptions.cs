namespace Yake.NET;

/// <summary>
/// Configuration options for the YAKE keyword extractor.
/// </summary>
public sealed class YakeOptions
{
    private int _maxNGramSize = 3;
    private int _topN = 10;
    private double _deduplicationThreshold = 0.9;
    private int _windowSize = 2;

    /// <summary>
    /// Maximum size of n-gram candidates (e.g. 2 = unigrams + bigrams).
    /// Must be >= 1. Default: 3
    /// </summary>
    public int MaxNGramSize
    {
        get => _maxNGramSize;
        set => _maxNGramSize = value >= 1
            ? value
            : throw new ArgumentOutOfRangeException(nameof(MaxNGramSize), "Must be >= 1.");
    }

    /// <summary>
    /// Number of top keywords to return.
    /// Must be >= 1. Default: 10
    /// </summary>
    public int TopN
    {
        get => _topN;
        set => _topN = value >= 1
            ? value
            : throw new ArgumentOutOfRangeException(nameof(TopN), "Must be >= 1.");
    }

    /// <summary>
    /// Deduplication threshold. Candidates with similarity above this are deduplicated.
    /// Must be between 0.0 and 1.0. Default: 0.9
    /// </summary>
    public double DeduplicationThreshold
    {
        get => _deduplicationThreshold;
        set => _deduplicationThreshold = value is >= 0.0 and <= 1.0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(DeduplicationThreshold), "Must be between 0.0 and 1.0.");
    }

    /// <summary>
    /// Window size used when computing co-occurrence relatedness.
    /// Must be >= 1. Default: 2
    /// </summary>
    public int WindowSize
    {
        get => _windowSize;
        set => _windowSize = value >= 1
            ? value
            : throw new ArgumentOutOfRangeException(nameof(WindowSize), "Must be >= 1.");
    }

    /// <summary>
    /// Optional custom stopwords. If null, the built-in English stopword list is used.
    /// </summary>
    public IEnumerable<string>? Stopwords { get; set; }
}
