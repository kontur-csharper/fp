using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagCloud.Settings;

namespace TagCloud.TextReaders
{
    public class FileReader : ITextReader
    {
        private readonly string filePath;
        
        public FileReader(FileReaderSettings settings)
        {
            filePath = settings.FilePath;
        }
        
        public Result<List<string>> ReadWords()
        {
            return Result.Of(() => File.ReadAllLines(filePath)
                .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToList(), "Cannot read file");
        }
    }
}