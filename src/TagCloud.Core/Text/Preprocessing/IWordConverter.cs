using System.Collections.Generic;
using FunctionalStuff.Results;

namespace TagCloud.Core.Text.Preprocessing
{
    public interface IWordConverter
    {
        Result<IEnumerable<string>> Normalize(IEnumerable<string> words);
    }
}