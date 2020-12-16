using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IFileReader
    {
        string Format { get; set; }
        Result<IEnumerable<string>> ReadAllLines(string filePath);
    }
}