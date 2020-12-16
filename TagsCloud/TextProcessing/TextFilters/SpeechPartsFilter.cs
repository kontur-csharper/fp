using System.Collections.Generic;
using System.Linq;
using ResultPattern;
using TagsCloud.TextProcessing.ParserForWordsAndSpeechParts;

namespace TagsCloud.TextProcessing.TextFilters
{
    public class SpeechPartsFilter : IWordsFilter
    {
        private readonly INormalizedWordAndSpeechPartParser _normalizedWordAndSpeechPartParser;
        private readonly HashSet<string> _boringSpeechParts;

        public SpeechPartsFilter(INormalizedWordAndSpeechPartParser normalizedWordAndSpeechPartParser,
            string[] boringSpeechPartsByMyStem)
        {
            _normalizedWordAndSpeechPartParser = normalizedWordAndSpeechPartParser;
            _boringSpeechParts = boringSpeechPartsByMyStem.ToHashSet();
        }

        public Result<string[]> GetInterestingWords(string[] words)
        {
            if (words == null)
                return new Result<string[]>("Array words for speech parts filter must be not null");
            var interestingWords = words
                .Select(word => _normalizedWordAndSpeechPartParser.ParseToNormalizedWordAndPartSpeech(word))
                .Where(wordAndPartSpeech =>
                    wordAndPartSpeech.GetValueOrThrow().Length != 0 &&
                    !_boringSpeechParts.Contains(wordAndPartSpeech.GetValueOrThrow()[1]))
                .Select(wordAndPartSpeech => wordAndPartSpeech.GetValueOrThrow()[0])
                .ToArray();
            return ResultExtensions.Ok(interestingWords);
        }
    }
}