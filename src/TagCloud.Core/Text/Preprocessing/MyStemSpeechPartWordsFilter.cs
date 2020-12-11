using System.Collections.Generic;
using System.Linq;
using FunctionalStuff.Results;
using FunctionalStuff.Results.Fails;
using MyStem.Wrapper.Workers.Grammar;
using MyStem.Wrapper.Workers.Grammar.Parsing;
using MyStem.Wrapper.Workers.Grammar.Parsing.Models;

namespace TagCloud.Core.Text.Preprocessing
{
    public class MyStemSpeechPartWordsFilter : IWordFilter
    {
        private readonly IGrammarAnalyser analyser;
        private readonly IGrammarAnalysisParser parser;
        private readonly ISet<MyStemSpeechPart> includedSpeechParts;

        public MyStemSpeechPartWordsFilter(IGrammarAnalyser analyser,
            IGrammarAnalysisParser parser, ISet<MyStemSpeechPart> includedSpeechParts)
        {
            this.analyser = analyser;
            this.parser = parser;
            this.includedSpeechParts = includedSpeechParts;
        }

        public Result<string[]> GetValidWordsOnly(IEnumerable<string> words)
        {
            return analyser.GetRawResult(string.Join(" ", words))
                .Then(x => Fail.If(x, $"{nameof(IGrammarAnalyser)} output").NullOrEmpty())
                .Then(x => x.Select(r => r.ParseWith(parser))
                    .Where(r => r.Entries.Any(e => includedSpeechParts.Contains(e.SpeechPart)))
                    .Select(r => r.Text)
                    .ToArray())
                .Then(x => Fail.If(x, "Words with specified speech part collection").NullOrEmpty())
                .RefineError("Cannot filter words by speech parts");
        }
    }
}