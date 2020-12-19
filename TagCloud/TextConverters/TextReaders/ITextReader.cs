using ResultOf;

namespace TagCloud.TextConverters.TextReaders
{
    public interface ITextReader
    {
        public string Extension { get; }

        public Result<string> ReadText(string path);
    }
}
