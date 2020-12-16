using ResultOf;

namespace TagCloud.TextProcessing
{
    public interface ITextReader
    {
        Result<string[]> ReadStrings(string pathToFile);
    }
}