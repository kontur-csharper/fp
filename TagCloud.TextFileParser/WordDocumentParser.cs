using System;
using System.IO;
using GemBox.Document;
using TagCloud.ExceptionHandler;

namespace TagCloud.TextFileParser
{
    public class WordDocumentParser : ITextFileParser
    {
        private const string LineSplitter = "\r\n";

        public Result<string[]> GetWords(string fileName, string sourceFolderPath)
        {
            if (Path.GetExtension(fileName) != ".docx" && Path.GetExtension(fileName) != ".doc")
            {
                return Result.Fail<string[]>("Некорректный формат файла с входными данными");
            }

            return Result.Of(() => DocumentModel.Load(Path.Combine(sourceFolderPath,
                    $"{fileName}")))
                .Then(document => document.Content.ToString())
                .Then(text => text.Split(LineSplitter, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}