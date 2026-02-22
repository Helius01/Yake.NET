using Yake.NET;

const string text = """
    Automatic keyword extraction uses statistical methods to extract words and phrases 
    that are most relevant to the topic of a document. YAKE is a novel feature-based 
    system for multi-lingual keyword extraction, which supports texts of different sizes, 
    domains, or languages. Unlike most approaches, YAKE does not rely on dictionaries, 
    thesauri, or pre-trained models of any kind, making it both lightweight and 
    language-agnostic. The method builds upon statistical text features to select the 
    most important keywords in a document. Machine learning and natural language 
    processing are key areas where keyword extraction is especially valuable.
    """;

Console.WriteLine("=== Yake.NET — Keyword Extraction Demo ===\n");
Console.WriteLine("Input text:");
Console.WriteLine(text);
Console.WriteLine();

// ── Example 1: Default settings ───────────────────────────────────────────────
Console.WriteLine("--- Top 10 keywords (default settings) ---");
var extractor = new YakeExtractor();
var keywords = extractor.Extract(text);

foreach (var kw in keywords)
    Console.WriteLine($"  {kw}");

Console.WriteLine();

// ── Example 2: Custom options ─────────────────────────────────────────────────
Console.WriteLine("--- Top 5 unigrams only ---");
var extractor2 = new YakeExtractor(new YakeOptions
{
    MaxNGramSize = 1,
    TopN = 5,
    DeduplicationThreshold = 0.7
});

var keywords2 = extractor2.Extract(text);

foreach (var kw in keywords2)
    Console.WriteLine($"  {kw}");

Console.WriteLine();

// ── Example 3: Custom stopwords ───────────────────────────────────────────────
Console.WriteLine("--- Custom stopwords (adding 'novel', 'method') ---");
var customStopwords = StopwordsEn.Words.Concat(["novel", "method"]);

var extractor3 = new YakeExtractor(new YakeOptions
{
    Stopwords = customStopwords,
    TopN = 5
});

var keywords3 = extractor3.Extract(text);

foreach (var kw in keywords3)
    Console.WriteLine($"  {kw}");
