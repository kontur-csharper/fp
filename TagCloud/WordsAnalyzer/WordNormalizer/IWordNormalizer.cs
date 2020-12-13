namespace TagCloud.WordsAnalyzer.WordNormalizer
{
    public interface IWordNormalizer
    {
        public Result<string> Normalize(string word);
    }
}