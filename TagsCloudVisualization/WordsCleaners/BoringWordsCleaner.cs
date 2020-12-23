using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHunspell;

namespace TagsCloudVisualization.WordsCleaners
{
    public class BoringWordsCleaner : IWordsCleaner
    {
        private HashSet<string> boringWords;

        public void AddBoringWords(HashSet<string> boringWords)
        {
            this.boringWords = boringWords;
        }

        public Result<List<string>> CleanWords(IEnumerable<string> words)
        {
            var ruDict = "ru_RU.dic";
            var ruAff = "ru_RU.aff";
            if (!File.Exists(ruDict) || !File.Exists(ruAff))
                return Result.Fail<List<string>>("there is no ruDict or ruAff in directore"); //TODO give normal name
            var hunspell = new Hunspell(ruAff, ruDict);
            var cleanedWords = words
                .Where(loweredWord => !boringWords.Contains(hunspell.Stem(loweredWord.ToLower()).FirstOrDefault()))
                .ToList();
            return cleanedWords;
        }
    }
}