using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Results;

namespace TagCloud.Core.Text.Preprocessing
{
    public class LengthWordFilter : IWordFilter
    {
        public Result<string[]> GetValidWordsOnly(IEnumerable<string> words) =>
            words.Where(word => word.Length >= 3)
                .ToArray()
                .AsResult();
    }
}