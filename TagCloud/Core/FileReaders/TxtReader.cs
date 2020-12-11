using System.Collections.Generic;
using System.IO;

namespace TagCloud.Core.FileReaders
{
    public class TxtReader : IFileReader
    {
        public FileExtension Extension => FileExtension.Txt;

        public Result<IEnumerable<string>> ReadAllWords(string filePath)
        {
            return File.Exists(filePath)
                ? File.ReadAllText(filePath).Split()
                : Result.Fail<IEnumerable<string>>($"File {filePath} not found");
        }
    }
}