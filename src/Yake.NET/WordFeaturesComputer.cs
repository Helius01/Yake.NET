namespace Yake.NET;

/// <summary>
/// Holds all per-word statistical features used by the YAKE scoring formula.
/// </summary>
internal sealed class WordFeatures
{
    /// <summary>Raw frequency count across the document.</summary>
    public int Frequency { get; set; }

    /// <summary>Capitalised occurrences count (not the first word of a sentence).</summary>
    public int UpperCount { get; set; }

    /// <summary>Positions (sentence index) where word first appears per sentence.</summary>
    public HashSet<int> SentencePositions { get; } = [];

    /// <summary>Set of different left-context words (words appearing before this word).</summary>
    public HashSet<string> LeftContext { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Set of different right-context words (words appearing after this word).</summary>
    public HashSet<string> RightContext { get; } = new(StringComparer.OrdinalIgnoreCase);

    // ── Computed scores ────────────────────────────────────────────────────────

    /// <summary>Casing score: ratio of capitalised (non-sentence-start) occurrences.</summary>
    public double TCasing { get; set; }

    /// <summary>Position score: 1 / log(3 + first-sentence index).</summary>
    public double TPosition { get; set; }

    /// <summary>Frequency score: normalised term frequency.</summary>
    public double TFrequency { get; set; }

    /// <summary>Relatedness to context: ratio of co-occurrence diversity to frequency.</summary>
    public double TRelatedness { get; set; }

    /// <summary>Sentence dispersion: fraction of sentences containing the word.</summary>
    public double TDifferentSentence { get; set; }

    /// <summary>Final single-word YAKE score (lower = more relevant).</summary>
    public double Score { get; set; }
}

/// <summary>
/// Computes per-word features from a tokenised document.
/// </summary>
internal static class WordFeaturesComputer
{
    internal static Dictionary<string, WordFeatures> Compute(
        List<List<string>> sentences,
        HashSet<string> stopwords,
        int windowSize)
    {
        var features = new Dictionary<string, WordFeatures>(StringComparer.OrdinalIgnoreCase);
        int totalSentences = sentences.Count;

        // ── Pass 1: collect raw counts ─────────────────────────────────────────
        for (int sIdx = 0; sIdx < sentences.Count; sIdx++)
        {
            var tokens = sentences[sIdx];

            for (int tIdx = 0; tIdx < tokens.Count; tIdx++)
            {
                string raw = tokens[tIdx];
                string lower = raw.ToLowerInvariant();

                // Skip stopwords — do NOT build features for them.
                // They remain in the token list so positional distance in the
                // context window is preserved, but we don't score them.
                if (stopwords.Contains(lower))
                    continue;

                if (!features.TryGetValue(lower, out var feat))
                {
                    feat = new WordFeatures();
                    features[lower] = feat;
                }

                feat.Frequency++;
                feat.SentencePositions.Add(sIdx);

                // Casing: count upper-case occurrences that are NOT the first
                // token of a sentence (to exclude sentence-opening capitalisation)
                if (tIdx > 0 && char.IsUpper(raw[0]))
                    feat.UpperCount++;

                // Context window — traverse the full token list (preserving real
                // positional distance), but only register non-stopword neighbours
                // so T_Relatedness reflects meaningful co-occurrences only.
                for (int w = 1; w <= windowSize; w++)
                {
                    if (tIdx - w >= 0)
                    {
                        var left = tokens[tIdx - w].ToLowerInvariant();
                        if (!stopwords.Contains(left))
                            feat.LeftContext.Add(left);
                    }

                    if (tIdx + w < tokens.Count)
                    {
                        var right = tokens[tIdx + w].ToLowerInvariant();
                        if (!stopwords.Contains(right))
                            feat.RightContext.Add(right);
                    }
                }
            }
        }

        // ── Pass 2: compute statistical scores ────────────────────────────────
        int maxFreq = features.Values.Max(f => f.Frequency);

        // Mean frequency (used for T_Frequency normalisation)
        double meanFreq = features.Values.Average(f => f.Frequency);

        // First sentence index per word (approximated as lowest sentence index)
        var firstSentenceIdx = features.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.SentencePositions.Min(),
            StringComparer.OrdinalIgnoreCase);

        foreach (var (word, feat) in features)
        {
            // T_Casing
            feat.TCasing = feat.UpperCount / (1.0 + Math.Log(feat.Frequency));

            // T_Position: earlier appearance → higher importance
            feat.TPosition = Math.Log(3.0 + firstSentenceIdx[word]);

            // T_Frequency: normalised against mean
            feat.TFrequency = feat.Frequency / (meanFreq + feat.Frequency);

            // T_Relatedness: diversity of co-occurring words relative to frequency
            double leftUniq = feat.LeftContext.Count;
            double rightUniq = feat.RightContext.Count;
            feat.TRelatedness = 1.0
                + (leftUniq + rightUniq) * (feat.Frequency / (double)maxFreq);

            // T_DifferentSentence: fraction of sentences the word appears in
            feat.TDifferentSentence = feat.SentencePositions.Count / (double)totalSentences;

            // Final word score (lower = more relevant)
            // Formula: (T_Position × T_Frequency) /
            //          (T_Casing + (T_Frequency / T_Relatedness) + T_DifferentSentence)
            double numerator = feat.TPosition * feat.TFrequency;
            double denominator = feat.TCasing
                + (feat.TFrequency / feat.TRelatedness)
                + feat.TDifferentSentence;

            feat.Score = numerator / (denominator == 0 ? double.Epsilon : denominator);
        }

        return features;
    }
}
