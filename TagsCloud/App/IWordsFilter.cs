using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public interface IWordsFilter
    {
        Result<bool> Validate(string word);
    }
}