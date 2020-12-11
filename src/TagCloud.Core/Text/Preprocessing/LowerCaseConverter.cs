using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Results;

namespace TagCloud.Core.Text.Preprocessing
{
    public class LowerCaseConverter : IWordConverter
    {
        public Result<IEnumerable<string>> Normalize(IEnumerable<string> words) =>
            words.Select(x => x.ToLower()).AsResult();
    }
}