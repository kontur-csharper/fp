using System.IO;
using TagCloud.ExceptionHandler;

namespace TagCloud.TextFileParser
{
    public class TxtFileParser : ITextFileParser
    {
        public Result<string[]> GetWords(string fileName, string sourceFolderPath)
        {
            if (Path.GetExtension(fileName) != ".txt")
            {
                return Result.Fail<string[]>("Некорректный формат файла с входными данными");
            }

            return Result.Of(() => File.ReadAllLines(Path.Combine(sourceFolderPath,
                $"{fileName}")));
        }
    }
}