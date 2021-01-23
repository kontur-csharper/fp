using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TagsCloudCreating.Core.WordProcessors
{
    public static class MyStemHandler
    {
        public static readonly Dictionary<PartsOfSpeech, string> BoringWords = new Dictionary<PartsOfSpeech, string>
        {
            [PartsOfSpeech.Adjective] = "A",
            [PartsOfSpeech.Adverb] = "ADV",
            [PartsOfSpeech.PronounAdverb] = "ADVPRO",
            [PartsOfSpeech.NumeralAdjective] = "ANUM",
            [PartsOfSpeech.PronounAdjective] = "APRO",
            [PartsOfSpeech.PartCompoundWord] = "COM",
            [PartsOfSpeech.Conjunction] = "CONJ",
            [PartsOfSpeech.Interjection] = "INTJ",
            [PartsOfSpeech.Numeral] = "NUM",
            [PartsOfSpeech.Particle] = "PART",
            [PartsOfSpeech.Preposition] = "PR",
            [PartsOfSpeech.Noun] = "S",
            [PartsOfSpeech.PronounNoun] = "SPRO",
            [PartsOfSpeech.Verb] = "V"
        };

        public static IEnumerable<(string word, string speechPart)> GetWordsWithPartsOfSpeech(IEnumerable<string> words)
        {
            const string tempInputFile = "input.txt";
            const string tempOutputFile = "output.json";
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "mystem.exe",
                    UseShellExecute = false,
                    Arguments = $@"-iln --format json {tempInputFile} {tempOutputFile}"
                }
            };

            File.WriteAllLines(tempInputFile, words);

            process.Start();
            process.WaitForExit();
            foreach (var rawJson in File.ReadAllLines(tempOutputFile))
                yield return ParseToWordTypePair(rawJson);

            File.Delete(tempInputFile);
            File.Delete(tempOutputFile);
        }

        private static (string word, string speechPart) ParseToWordTypePair(string rawJson)
        {
            var jsonDoc = JsonDocument.Parse(rawJson);
            var (word, type) = jsonDoc.RootElement
                .GetProperty("analysis")
                .EnumerateArray()
                .Select(GetWordAndTypeTuple)
                .FirstOrDefault();
            return (word, type);

            static (string, string) GetWordAndTypeTuple(JsonElement jsonElement)
            {
                var normalizedWord = jsonElement.GetProperty("lex").GetString();
                var type = jsonElement.GetProperty("gr").GetString().Split(',', '=').First();
                return (normalizedWord, type);
            }
        }
    }
}