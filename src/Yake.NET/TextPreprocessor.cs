using System.Text.RegularExpressions;

namespace Yake.NET;

/// <summary>
/// Splits raw text into sentences and tokens for YAKE processing.
/// </summary>
internal static class TextPreprocessor
{
    // Sentence boundary: period/exclamation/question followed by whitespace + capital letter
    private static readonly Regex SentenceSplitter =
        new(@"(?<=[.!?])\s+(?=[A-Z])", RegexOptions.Compiled);

    // Tokenizer: extract word-like tokens (letters, digits, hyphens inside words)
    private static readonly Regex TokenPattern =
        new(@"\b[\w][\w\-']*\b", RegexOptions.Compiled);

    /// <summary>
    /// Splits text into sentences, each sentence being a list of raw tokens.
    /// </summary>
    internal static List<List<string>> Tokenize(string text)
    {
        var sentences = SentenceSplitter.Split(text.Trim());
        var result = new List<List<string>>(sentences.Length);

        foreach (var sentence in sentences)
        {
            var tokens = TokenPattern.Matches(sentence)
                                     .Select(m => m.Value)
                                     .ToList();
            if (tokens.Count > 0)
                result.Add(tokens);
        }

        return result;
    }
}
