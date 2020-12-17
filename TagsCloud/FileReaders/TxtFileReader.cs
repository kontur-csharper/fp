using System;
using System.IO;
using System.Linq;
using ResultOf;

namespace TagsCloud.FileReaders
{
    public class TxtFileReader : IFileReader
    {
        public Result<string[]> GetWordsFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return Result.Fail<string[]>("File not exists");

            try
            {
                var result = File.ReadLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                return Result.Ok(result);
            }
            catch (Exception)
            {
                return new Result<string[]>("Не удалось прочитать файл");
            }
        }
    }
}
