using System.Collections.Generic;
using System.IO;
using TagsCloud.WordSelectors;

namespace TagsCloud.WordReaders
{
    public class ByLineWordReader : IWordReader
    {
        private readonly string filePath;
        private readonly IWordSelector selector;

        public ByLineWordReader(string filePath, IWordSelector selector)
        {
            this.filePath = filePath;
            this.selector = selector;
        }

        public Result<IEnumerable<string>> ReadWords() => Result
            .Of<IEnumerable<string>>(() => File.ReadAllLines(filePath))
            .Then(words => selector.TakeSelectedWords(words));
    }
}