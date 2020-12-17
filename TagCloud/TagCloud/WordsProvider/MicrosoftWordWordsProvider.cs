using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public class MicrosoftWordWordsProvider : FileWordsProvider
    {
        private MicrosoftWordWordsProvider(string filePath) : base(filePath)
        {
            SupportedExtensions = new[] {"doc", "docx"};
        }

        public override string[] SupportedExtensions { get; }

        public override Result<IEnumerable<string>> GetWords()
        {
            if (!CheckFile(FilePath))
                return Result.Fail<IEnumerable<string>>(IncorrectFileExceptionMessage());
            var application = new Application();
            var document = application.Documents.Open(FilePath);
            var words = (from Range word in document.Words
                    where Regex.IsMatch(word.Text, @"\w+")
                    select word.Text)
                .ToList();
            application.Quit();
            return words;
        }

        public static Result<IWordsProvider> Create(string filePath)
        {
            var result = new MicrosoftWordWordsProvider(filePath);
            if (!result.CheckFile(filePath))
                return Result.Fail<IWordsProvider>(result.IncorrectFileExceptionMessage());
            return result;
        }
    }
}