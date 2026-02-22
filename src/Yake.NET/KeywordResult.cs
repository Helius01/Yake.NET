namespace Yake.NET;

/// <summary>
/// Represents an extracted keyword with its YAKE score.
/// Lower score = more relevant keyword.
/// </summary>
public sealed class KeywordResult
{
    /// <summary>The extracted keyword or keyphrase.</summary>
    public string Keyword { get; }

    /// <summary>
    /// YAKE relevance score. Lower is better.
    /// </summary>
    public double Score { get; }

    public KeywordResult(string keyword, double score)
    {
        Keyword = keyword;
        Score = score;
    }

    public override string ToString() => $"{Keyword} (score: {Score:F6})";
}
