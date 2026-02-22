namespace Yake.NET;

/// <summary>
/// Removes near-duplicate keyword candidates using normalised Levenshtein similarity.
/// </summary>
internal static class Deduplicator
{
    /// <summary>
    /// Returns a deduplicated list, retaining the best (lowest) scoring candidate
    /// when two candidates are too similar.
    /// </summary>
    internal static List<(string Candidate, double Score)> Deduplicate(
        List<(string Candidate, double Score)> candidates,
        double threshold)
    {
        var result = new List<(string, double)>();

        foreach (var current in candidates)
        {
            bool isDuplicate = result.Any(existing =>
                Similarity(existing.Item1, current.Candidate) > threshold);

            if (!isDuplicate)
                result.Add(current);
        }

        return result;
    }

    /// <summary>
    /// Computes normalised Levenshtein similarity between two strings.
    /// Returns a value between 0.0 (completely different) and 1.0 (identical).
    /// </summary>
    private static double Similarity(string a, string b)
    {
        if (a.Equals(b, StringComparison.OrdinalIgnoreCase)) return 1.0;

        int maxLen = Math.Max(a.Length, b.Length);
        if (maxLen == 0) return 1.0;

        // Early exit: if the length difference alone guarantees the similarity
        // can't exceed 0.5, skip the expensive O(m*n) Levenshtein computation.
        int minLen = Math.Min(a.Length, b.Length);
        double maxPossibleSimilarity = (double)minLen / maxLen;
        if (maxPossibleSimilarity < 0.5) return maxPossibleSimilarity;

        int distance = LevenshteinDistance(a, b);
        return 1.0 - (distance / (double)maxLen);
    }

    private static int LevenshteinDistance(string a, string b)
    {
        int m = a.Length, n = b.Length;
        int[] prev = new int[n + 1];
        int[] curr = new int[n + 1];

        for (int j = 0; j <= n; j++) prev[j] = j;

        for (int i = 1; i <= m; i++)
        {
            curr[0] = i;

            for (int j = 1; j <= n; j++)
            {
                int cost = char.ToLowerInvariant(a[i - 1]) == char.ToLowerInvariant(b[j - 1])
                    ? 0 : 1;

                curr[j] = Math.Min(
                    Math.Min(prev[j] + 1, curr[j - 1] + 1),
                    prev[j - 1] + cost);
            }

            (prev, curr) = (curr, prev);
        }

        return prev[n];
    }
}
