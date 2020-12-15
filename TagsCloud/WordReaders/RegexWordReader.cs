using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagsCloud.WordSelectors;

namespace TagsCloud.WordReaders
{
    public class RegexWordReader : IWordReader
    {
        private static readonly Regex RegexReader = new Regex("\\W+", RegexOptions.Compiled);
        
        private readonly string filePath;
        private readonly IWordSelector selector;

        public RegexWordReader(string filePath, IWordSelector selector)
        {
            this.filePath = filePath;
            this.selector = selector;
        }

        public Result<IEnumerable<string>> ReadWords() => Result
            .Of(() => File.ReadAllText(filePath))
            .Then(text => RegexReader.Split(text))
            .Then(words => selector.TakeSelectedWords(words));
    }
}