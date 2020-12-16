using ResultPattern;

namespace TagsCloud.TextProcessing.TextFilters
{
    public interface IWordsFilter
    {
        Result<string[]> GetInterestingWords(string[] words);
    }
}