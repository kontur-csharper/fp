using System.Collections.Generic;

namespace TagCloud.Core.FileReaders
{
    public interface IFileReader
    {
        FileExtension Extension { get; }
        Result<IEnumerable<string>> ReadAllWords(string filePath);
    }
}