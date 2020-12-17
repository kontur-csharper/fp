using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MyStemWrapper;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class WordTypeConveyor : IConveyor
    {
        private readonly MyStem analyzer;
        private readonly Regex wordWithTypeRegex;

        public WordTypeConveyor(string myStemPath)
        {
            wordWithTypeRegex = new Regex(@".+?\{(?<word>.+?)=(?<type>.+?)\W.+?\}");
            analyzer = new MyStem {PathToMyStem = myStemPath, Parameters = "-i"};
        }

        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens)
        {
            var analysis = analyzer.Analysis(string.Join(" ", tokens.Select(pair => pair.Token)));
            foreach (Match match in wordWithTypeRegex.Matches(analysis))
            {
                var word = match.Groups["word"].Value;
                if (match.Success)
                {
                    if (!Enum.TryParse(match.Groups["type"].Value, out WordType wordType))
                        wordType = WordType.UNKNOWN;
                    yield return new TokenInfo(word, wordType);
                }
            }
        }
    }
}