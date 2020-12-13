namespace TagCloud.WordsAnalyzer.WordFilters
{
    public interface IWordFilter
    {
        public Result<bool> ShouldExclude(string word);
    }
}