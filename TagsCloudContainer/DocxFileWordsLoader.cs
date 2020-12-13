using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TagsCloudContainer
{
    public class DocxFileWordsLoader : IWordsLoader
    {
        private readonly string pathToFile;

        public DocxFileWordsLoader(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }

        public string[] GetWords()
        {
            var words = new List<string>();
            using (var wordDocument = WordprocessingDocument.Open(pathToFile, false))
            {
                var paragraphs = wordDocument.MainDocumentPart.Document.Body.OfType<Paragraph>();
                words.AddRange(paragraphs.Select(paragraph => paragraph.InnerText));
            }

            return words.ToArray();
        }
    }
}