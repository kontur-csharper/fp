namespace TagCloud.WordsAnalyzer.WordNormalizer
{
    public class WordNormalizer : IWordNormalizer
    {
        public Result<string> Normalize(string word)
        {
            return Result.Ok(word.ToLower());
        }
    }
}