using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using ResultOf;

namespace TagCloud.TextProcessing
{
    public class DocxTextReader : ITextReader
    {
        public Result<string[]> ReadStrings(string pathToFile)
        {
            WordprocessingDocument doc;
            try
            {
                doc = WordprocessingDocument.Open(pathToFile, false);
            }
            catch (FileNotFoundException)
            {
                return Result.Fail<string[]>("Is directory / Directory not found " + pathToFile);
            }
            catch (Exception e)
            {
                return Result.Fail<string[]>("Unhahandled exception " + e);
            }
            
            return doc.MainDocumentPart.Document.Body
                .Select(item => item.InnerText).ToArray();
        }
    }
}