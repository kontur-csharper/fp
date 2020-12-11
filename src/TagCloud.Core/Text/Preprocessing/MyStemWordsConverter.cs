using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;
using MyStem.Wrapper.Workers.Lemmas;

namespace TagCloud.Core.Text.Preprocessing
{
    public class MyStemWordsConverter : IWordConverter
    {
        private readonly ILemmatizer normalizer;

        public MyStemWordsConverter(ILemmatizer normalizer)
        {
            this.normalizer = normalizer;
        }

        public Result<IEnumerable<string>> Normalize(IEnumerable<string> words) =>
            Fail.If(words, $"{nameof(MyStemWordsConverter)} input").NullOrEmpty()
                .Then(w => normalizer.GetWords(string.Join(" ", w)))
                .Then(c => Fail.If(c, $"{nameof(ILemmatizer)} output").NullOrEmpty())
                .Then(r => r.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimEnd('?')));
    }
}