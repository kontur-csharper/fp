using System.Collections.Generic;
using System.Linq;
using TagCloud.ExceptionHandler;

namespace TagCloud.TextFileParser
{
    public class FileParser : ITextFileParser
    {
        private readonly IEnumerable<ITextFileParser> implementations;

        public FileParser(IEnumerable<ITextFileParser> implementations)
        {
            this.implementations = implementations;
        }

        public Result<string[]> GetWords(string fileName, string sourceFolderPath)
        {
            var allResults = implementations
                .Select(parser => parser.GetWords(fileName, sourceFolderPath))
                .ToArray();
            var successResult = Result.Of(() => allResults.First(result => result.IsSuccess));
            return successResult.IsSuccess ? successResult.GetValueOrThrow() : allResults[0];
        }
    }
}