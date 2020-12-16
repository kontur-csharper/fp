using System.Text.RegularExpressions;
using ResultPattern;

namespace TagsCloud.TextProcessing.ParserForWordsAndSpeechParts
{
    public class NormalizedWordAndSpeechPartParser : INormalizedWordAndSpeechPartParser
    {
        private static Regex _regex = new Regex(@"\w+{(\w+=\w+).*}");

        public Result<string[]> ParseToNormalizedWordAndPartSpeech(string word)
        {
            if (word == null)
                return new Result<string[]>("String for parse to word and part of speech must be not null");
            var match = _regex.Match(word);
            var normalizedWordsAndPartSpeech = !match.Success ? new string[0] : match.Groups[1].Value.Split('=');
            return ResultExtensions.Ok(normalizedWordsAndPartSpeech);
        }
    }
}