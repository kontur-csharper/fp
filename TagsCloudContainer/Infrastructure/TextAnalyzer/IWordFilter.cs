using ResultOf;

namespace TagsCloudContainer.Infrastructure.TextAnalyzer
{
    public interface IWordFilter
    {
        public Result<bool> IsBoring(string word);
    }
}