using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloud.Options;
using TagsCloud.ResultOf;

namespace TagsCloud.WordsParser
{
    public class Filter : IFilter
    {
        private static readonly List<string> BoringPartsOfSpeech =
            new List<string> {"=CONJ", "=INTJ", "=PART", "=PR"};

        private static readonly Regex MatchWordsInfo = new Regex(@"(?<wordInfo>\w+{.*?})");
        private readonly HashSet<string> boringWords;
        private readonly string mystemLocation;

        public Filter(IFilterOptions options)
        {
            boringWords = options.BoringWords.Select(word => word.Normalize()).ToHashSet();
            mystemLocation = options.MystemLocation;
        }

        public Result<IEnumerable<string>> RemoveBoringWords(IEnumerable<string> words) =>
            Result.Of(() => GetWordsInfo(words.ToHashSet(), mystemLocation))
                .Then(wordsInfo => words.Where(word => !IsBoringWord(word, wordsInfo)));

        private bool IsBoringWord(string word, IEnumerable<string> wordsInfo)
            => boringWords.Contains(word) || IsWordBoringPartOfSpeech(word, wordsInfo);

        private static bool IsWordBoringPartOfSpeech(string word, IEnumerable<string> wordsInfo)
        {
            return wordsInfo.Where(wordInfo => wordInfo.Contains(word))
                .Any(wordInfo => BoringPartsOfSpeech.Any(wordInfo.Contains));
        }

        private static IEnumerable<string> GetWordsInfo(IEnumerable<string> words, string mystemLocation)
        {
            if (mystemLocation == "")
                return words;

            var mystem = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(mystemLocation),
                    Arguments = "-i -n -e cp866",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };
            mystem.Start();

            foreach (var word in words)
                mystem.StandardInput.Write($"{word}\n");
            mystem.StandardInput.Close();

            var wordsInfo = MatchWordsInfo
                .Matches(mystem.StandardOutput.ReadToEnd())
                .Select(m => m.Groups["wordInfo"].Value)
                .ToList();
            mystem.WaitForExit();
            mystem.Close();
            mystem.Dispose();
            return wordsInfo;
        }
    }
}