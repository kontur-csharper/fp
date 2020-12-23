using System.Collections.Generic;
using System.IO;

namespace TagsCloudContainer
{
    public class WordsSourceFromFile : IWordsSource
    {
        public readonly string FileName;

        public WordsSourceFromFile(string fileName)
        {
            FileName = fileName;
        }

        public Result<IEnumerable<(string word, int count)>> GetWords()
        {
            if (!File.Exists(FileName))
                return Result.Fail<IEnumerable<(string WordRendererToImage, int count)>>(
                    $"Не удаётся прочитать файл {FileName}");
            return new WordsSourceFromText(File.ReadAllText(FileName)).GetWords();
        }
    }
}