using ResultOf;

namespace TagsCloudContainer.Infrastructure.TextAnalyzer
{
    public interface IWordNormalizer
    {
        public Result<string> NormalizeWord(string word);
    }
}