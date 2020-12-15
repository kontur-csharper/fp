using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using ResultOf;
using TagsCloudContainer.Infrastructure.DataReader;

namespace TagsCloudContainer.App.DataReader
{
    public class WordFileReader : IDataReader
    {
        private readonly string filename;

        public WordFileReader(string filename)
        {
            this.filename = filename;
        }

        public Result<IEnumerable<string>> ReadLines()
        {
            if (!File.Exists(filename))
                return Result.Fail<IEnumerable<string>>("Input file is not found");
            return Result.Of(() => WordprocessingDocument
                    .Open(filename, false)
                    .MainDocumentPart
                    .Document
                    .Body.Select(item => item.InnerText))
                .RefineError("Can't read input file");
            ;
        }
    }
}