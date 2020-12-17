using ResultOf;

namespace TagsCloud.FileReaders
{
    public interface IFileReader
    {
        public Result<string[]> GetWordsFromFile(string filePath);
    }
}