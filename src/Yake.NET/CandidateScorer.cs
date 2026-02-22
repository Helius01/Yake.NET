namespace Yake.NET;

/// <summary>
/// Generates n-gram keyword candidates and scores them using per-word YAKE scores.
/// </summary>
internal static class CandidateScorer
{
    /// <summary>
    /// Extracts scored candidates (unigrams through n-grams) from the tokenised document.
    /// </summary>
    internal static List<(string Candidate, double Score)> Score(
        List<List<string>> sentences,
        Dictionary<string, WordFeatures> wordFeatures,
        HashSet<string> stopwords,
        int maxNGramSize)
    {
        var candidates = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        foreach (var tokens in sentences)
        {
            for (int start = 0; start < tokens.Count; start++)
            {
                for (int n = 1; n <= maxNGramSize; n++)
                {
                    if (start + n > tokens.Count) break;

                    var ngram = tokens.GetRange(start, n);

                    // ── Validity checks ────────────────────────────────────────
                    // First and last token must not be stopwords
                    if (stopwords.Contains(ngram[0].ToLowerInvariant())) break;
                    if (stopwords.Contains(ngram[^1].ToLowerInvariant())) continue;

                    // Candidate must contain at least one non-stopword
                    if (ngram.All(t => stopwords.Contains(t.ToLowerInvariant()))) continue;

                    // Skip pure numeric tokens
                    if (ngram.All(t => double.TryParse(t, out _))) continue;

                    string candidate = string.Join(" ", ngram.Select(t => t.ToLowerInvariant()));

                    // ── Score: product of word scores / n² ────────────────────
                    double score = ComputeNGramScore(ngram, wordFeatures, n);

                    // Keep lowest (best) score if candidate seen multiple times
                    if (!candidates.TryGetValue(candidate, out double existing)
                        || score < existing)
                    {
                        candidates[candidate] = score;
                    }
                }
            }
        }

        return candidates
            .Select(kvp => (kvp.Key, kvp.Value))
            .OrderBy(x => x.Value)
            .ToList();
    }

    private static double ComputeNGramScore(
        List<string> ngram,
        Dictionary<string, WordFeatures> wordFeatures,
        int n)
    {
        double product = 1.0;
        double sum = 0.0;

        foreach (var token in ngram)
        {
            string lower = token.ToLowerInvariant();

            double ws = wordFeatures.TryGetValue(lower, out var feat)
                ? feat.Score
                : 1.0; // unknown words get neutral score

            product *= ws;
            sum += ws;
        }

        // YAKE n-gram formula: product(word_scores) / (n * (n + sum_word_scores))
        return product / (n * (n + sum));
    }
}
