using System.Collections.Generic;

namespace TagCloud.TextReaders
{
    public interface ITextReader
    {
        public Result<List<string>> ReadWords();
    }
}
