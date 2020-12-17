using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public class TxtWordsProvider : FileWordsProvider
    {
        public override string[] SupportedExtensions { get; }

        public override Result<IEnumerable<string>> GetWords()
        {
            if (!CheckFile(FilePath))
                return Result.Fail<IEnumerable<string>>($"Incorrect file {FilePath}");
            var words = Result.Of<IEnumerable<string>>(() =>
                Regex.Split(File.ReadAllText(FilePath), @"\W+"));
            return words;
        }

        public static Result<IWordsProvider> Create(string filePath)
        {
            var result = new TxtWordsProvider(filePath);
            if (!result.CheckFile(filePath))
                return Result.Fail<IWordsProvider>(result.IncorrectFileExceptionMessage());
            return result;
        }

        private TxtWordsProvider(string filePath) : base(filePath)
        {
            SupportedExtensions = new[] {"txt"};
        }
    }
}