using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    class FileReaderTxt : IFileReader
    {
        public string Format { get; set; }

        public FileReaderTxt()
        {
            Format = "txt";
        }

        public Result<IEnumerable<string>> ReadAllLines(string filePath)
        {
            return Result.Of(() => File.ReadAllLines(filePath).Where(word => word != ""), nameof(FileReaderTxt))
                    .RefineError("Не удалось прочитать слова из файла");
        }
    }
}
