using System.Collections.Generic;
using ResultOf;

namespace TagsCloud.WordFilters
{
    interface IWordFilter
    {
        public Result<IReadOnlyCollection<string>> FilterWords(IEnumerable<string> words);
    }
}