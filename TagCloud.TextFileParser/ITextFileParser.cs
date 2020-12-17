using TagCloud.ExceptionHandler;

namespace TagCloud.TextFileParser
{
    public interface ITextFileParser
    {
        public Result<string[]> GetWords(string fileName, string sourceFolderPath);
    }
}