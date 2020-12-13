using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagsCloud.ResultOf;

namespace TagsCloud.WordsParser
{
    public class FileReader : IWordReader
    {
        private static readonly Regex ExtensionRegex = new Regex(@".*?(?<extension>\.[^.]*?)$");
        private readonly string path;

        private delegate Result<IEnumerable<string>> ReadWordsMethod();

        public FileReader(string filePath)
        {
            path = filePath;
        }

        public Result<IEnumerable<string>> ReadWords() =>
            GetFileExtension(path)
                .Then(GetReadFileMethod)
                .Then(readWords => readWords.Invoke());

        private Result<IEnumerable<string>> ReadWordsFromTxt() =>
            CheckFileExisting().Then(_ => File.ReadLines(path));

        private Result<bool> CheckFileExisting()
            => File.Exists(path) ? true : Result.Fail<bool>($"File {path} not found.");

        private Result<ReadWordsMethod> GetReadFileMethod(string extension)
        {
            return extension switch
            {
                ".txt" => Result.Ok<ReadWordsMethod>(ReadWordsFromTxt),
                "" => Result.Fail<ReadWordsMethod>("Set file extension."),
                _ => Result.Fail<ReadWordsMethod>($"Can't read {extension} file")
            };
        }

        private static Result<string> GetFileExtension(string filePath) =>
            Result.Of(() => ExtensionRegex.Match(filePath).Groups["extension"].Value)
                .ReplaceError(e => $"Incorrect extension on file {filePath}");
    }
}