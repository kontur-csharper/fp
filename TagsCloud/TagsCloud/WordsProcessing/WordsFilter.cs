using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagsCloud.WordsProcessing
{
    public class WordsFilter : IWordsFilter
    {
        private HashSet<string> wordsToIgnore;
        public string regexWordPattern { get; set; }
        public WordsFilter(ExcludingWordsConfigurator configurator, string regexWordPattern = "^\\w+$")
        {
            wordsToIgnore = configurator.ExcludedWords;
            this.regexWordPattern = regexWordPattern;
        }

        public Result<IEnumerable<string>> GetCorrectWords(IEnumerable<string> words)
        {
            return Result.Of(() => 
                words.Where(x => !wordsToIgnore.Contains(x))
                    .Select(CheckFormat));
        }

        private string CheckFormat(string word)
        {
            if (!Regex.IsMatch(word, regexWordPattern))
                throw new FormatException("Входной файл должен содержать только одно слово в строке");
            return word;
        }
    }
}
