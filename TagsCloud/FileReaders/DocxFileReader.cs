using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using ResultOf;

namespace TagsCloud.FileReaders
{
    public class DocxFileReader : IFileReader
    {
        public Result<string[]> GetWordsFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return Result.Fail<string[]>("File not exists");

            var document = WordprocessingDocument.Open(filePath, false);

            var documentBody = document.MainDocumentPart.Document.Body;

            return documentBody
                .Select(item => item.InnerText)
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                .ToArray().AsResult();
        }
    }
}
