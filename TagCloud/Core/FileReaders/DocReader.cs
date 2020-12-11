using System.Collections.Generic;
using System.IO;
using Xceed.Words.NET;

namespace TagCloud.Core.FileReaders
{
    public class DocReader : IFileReader
    {
        public FileExtension Extension => FileExtension.Doc;

        public Result<IEnumerable<string>> ReadAllWords(string filePath)
        {
            return File.Exists(filePath)
                ? DocX.Load(filePath).Text.Split()
                : Result.Fail<IEnumerable<string>>($"File {filePath} not found");
        }
    }
}