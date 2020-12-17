using System.Collections.Generic;
using System.Text.RegularExpressions;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public abstract class FileReader : IFileReader
    {
        protected Regex splitRegex = new Regex("\\W+");
        public virtual HashSet<string> AvailableFileTypes { get; }

        public Result<string[]> ReadWords(string fileName)
        {
            var fileType = fileName.Split('.')[^1];
            if (!AvailableFileTypes.Contains(fileType))
                return Result.Fail<string[]>($"Incorrect type {fileType}");
            return ReadWordsInternal(fileName);
        }

        protected abstract Result<string[]> ReadWordsInternal(string fileName);
    }
}