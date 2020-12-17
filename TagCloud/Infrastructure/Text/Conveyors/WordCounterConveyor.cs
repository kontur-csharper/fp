using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class WordCounterConveyor : IConveyor
    {
        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens)

        {
            tokens = tokens.ToList();
            var counts = GetCount(tokens.Select(pair => pair.Token));
            foreach (var word in counts.Keys)
            {
                var info = tokens.First(pair => pair.Token == word);
                info.Frequency = counts[word];
                yield return info;
            }
        }

        private static Dictionary<string, int> GetCount(IEnumerable<string> tokens) => tokens
            .GroupBy(token => token)
            .ToDictionary(
                x => x.Key,
                x => x.Count());
    }
}