using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public abstract class FileWordsProvider : IWordsProvider
    {
        protected FileWordsProvider(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

        public abstract string[] SupportedExtensions { get; }

        public abstract Result<IEnumerable<string>> GetWords();

        protected string IncorrectFileExceptionMessage()
        {
            return $"Incorrect file {FilePath}";
        }

        protected bool CheckFile(string filePath)
        {
            return File.Exists(filePath) &&
                   SupportedExtensions.Any(extension => filePath.EndsWith(extension));
        }
    }
}