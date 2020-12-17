using System;
using System.Collections.Generic;
using Xceed.Words.NET;

namespace TagsCloud.TextProcessing.TextReaders
{
    public class DocReader : IWordsReader
    {
        public bool CanRead(string path) =>
            path.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".docx", StringComparison.OrdinalIgnoreCase);

        public IEnumerable<string> ReadWords(string path) => DocX.Load(path).Text.Split(' ', '\n');
    }
}
