using ResultPattern;

namespace TagsCloud.TextProcessing.ParserForWordsAndSpeechParts
{
    public interface INormalizedWordAndSpeechPartParser
    {
        Result<string[]> ParseToNormalizedWordAndPartSpeech(string text);
    }
}