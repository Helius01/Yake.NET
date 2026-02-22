namespace Yake.NET;

/// <summary>
/// Built-in English stopwords list used when no custom stopwords are supplied.
/// </summary>
/// <summary>Built-in English stopwords. Can be used as a base when supplying custom stopwords.</summary>
public static class StopwordsEn
{
    /// <summary>The built-in English stopword set.</summary>
    public static readonly HashSet<string> Words = new(StringComparer.OrdinalIgnoreCase)
    {
        "a", "about", "above", "after", "again", "against", "all", "am", "an", "and",
        "any", "are", "aren't", "as", "at", "be", "because", "been", "before", "being",
        "below", "between", "both", "but", "by", "can't", "cannot", "could", "couldn't",
        "did", "didn't", "do", "does", "doesn't", "doing", "don't", "down", "during",
        "each", "few", "for", "from", "further", "get", "got", "had", "hadn't", "has",
        "hasn't", "have", "haven't", "having", "he", "he'd", "he'll", "he's", "her",
        "here", "here's", "hers", "herself", "him", "himself", "his", "how", "how's",
        "i", "i'd", "i'll", "i'm", "i've", "if", "in", "into", "is", "isn't", "it",
        "it's", "its", "itself", "just", "let's", "me", "more", "most", "mustn't", "my",
        "myself", "no", "nor", "not", "of", "off", "on", "once", "only", "or", "other",
        "ought", "our", "ours", "ourselves", "out", "over", "own", "same", "shan't",
        "she", "she'd", "she'll", "she's", "should", "shouldn't", "so", "some", "such",
        "than", "that", "that's", "the", "their", "theirs", "them", "themselves", "then",
        "there", "there's", "these", "they", "they'd", "they'll", "they're", "they've",
        "this", "those", "through", "to", "too", "under", "until", "up", "very", "was",
        "wasn't", "we", "we'd", "we'll", "we're", "we've", "were", "weren't", "what",
        "what's", "when", "when's", "where", "where's", "which", "while", "who", "who's",
        "whom", "why", "why's", "will", "with", "won't", "would", "wouldn't", "you",
        "you'd", "you'll", "you're", "you've", "your", "yours", "yourself", "yourselves",
        "also", "however", "whereas", "therefore", "thus", "hence", "although", "though",
        "since", "whether", "either", "neither", "yet", "still", "already", "else",
        "elsewhere", "everywhere", "anything", "anyone", "everyone", "everything",
        "nothing", "nobody", "someone", "something", "somewhere", "nowhere", "upon",
        "within", "without", "along", "among", "beside", "besides", "beyond", "despite",
        "except", "inside", "outside", "regarding", "concerning", "considering",
        "according", "like", "unlike", "near", "next", "far", "back", "front",
        "around", "across", "behind", "ahead", "below", "above", "left", "right",
        "using", "used", "use", "make", "made", "made", "take", "taken", "given",
        "give", "gives", "show", "shows", "shown", "find", "found", "see", "seen",
        "know", "known", "include", "includes", "included", "may", "might", "shall",
        "must", "need", "needs", "seem", "seems", "seemed", "become", "becomes",
        "became", "keep", "kept", "let", "say", "said", "go", "goes", "went", "gone",
        "come", "came", "put", "set", "sets", "look", "looked", "looks", "well",
        "even", "much", "many", "new", "old", "now", "then", "here", "there",
        "always", "never", "often", "usually", "sometimes", "perhaps", "maybe",
        "quite", "rather", "almost", "enough", "really", "very", "too", "so",
        "hereby", "thereof", "thereby", "therein", "wherein", "whereby", "herein"
    };
}
