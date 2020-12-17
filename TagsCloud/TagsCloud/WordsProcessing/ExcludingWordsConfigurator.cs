using System.Collections.Generic;
using System.Linq;

namespace TagsCloud.WordsProcessing
{
    public class ExcludingWordsConfigurator
    {
        public HashSet<string> ExcludedWords { get; }
        public ExcludingWordsConfigurator(IEnumerable<string> wordsToExclude) =>
            ExcludedWords = wordsToExclude.ToHashSet();
    }
}