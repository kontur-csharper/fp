using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagCloud.Interfaces;

namespace TagCloud
{
    public class WordsReader : IWordsReader
    {
        public Result<List<string>> Get(string path)
        {
            return Result.Of(() =>
            {
                using (var fileStream = new StreamReader(path))
                {
                    return fileStream.ReadToEnd().Split('\n').ToList();
                }
            }).ReplaceError(x => $"Can't open file: {path}");
        }
    }
}