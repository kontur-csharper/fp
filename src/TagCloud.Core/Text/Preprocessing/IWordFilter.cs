using System.Collections.Generic;
using FunctionalStuff.Results;

namespace TagCloud.Core.Text.Preprocessing
{
    public interface IWordFilter
    {
        Result<string[]> GetValidWordsOnly(IEnumerable<string> word);
    }
}