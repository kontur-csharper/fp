using ResultPattern;

namespace TagsCloud.Reader
{
    public interface IFileReader
    {
        Result<string> GetTextFromFile();
    }
}