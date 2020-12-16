using ResultOf;

namespace TagCloud.TextProcessing
{
    public interface IWordParser
    {
        Result<string[]> GetWords(string fileName);
    }
}