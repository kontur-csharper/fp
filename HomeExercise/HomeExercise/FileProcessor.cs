using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResultOf;

namespace HomeExercise
{
    public class FileProcessor : IFileProcessor
    {
        private static char[] punctuationMarks = {'.', ',', '?', '!', ':', ':', '-', '—', '«', '»', '[', ']', '(', ')', '{','}','„','“'};
        private readonly string pathWords;
        private readonly string pathBoringWords;
        private List<string> excludedWords = new List<string>();
        
        public FileProcessor(string pathWords, string pathBoringWords)
        {
            this.pathWords = pathWords;
            this.pathBoringWords = pathBoringWords;
        }

        public Result<Dictionary<string, int>> GetWords()
        {
            var resultWords = Result.Of(() => ExtractTextFromFile(pathWords)).OnFail(Console.WriteLine);
            if (!resultWords.IsSuccess)
                return Result.Fail<Dictionary<string, int>>(resultWords.Error).RefineError(ToString());

            if (pathBoringWords != null)
            {
                var resultExludedWords = Result.Of(() => ExtractTextFromFile(pathBoringWords)).OnFail(Console.WriteLine);
                if(!resultExludedWords.IsSuccess)
                    return Result.Fail<Dictionary<string, int>>(resultExludedWords.Error);
                excludedWords = resultExludedWords.Value;
            }

            return resultWords.Then(FilterWords).Then(GetFrequencyWords).OnFail(Console.WriteLine);
        }

        private Result<string[]> FilterWords(List<string> words)
        {
            if (words == null) 
                Result.Fail<string[]>("Words not implemented");
            
            return Result.Of((() => words
                .Where(w => !excludedWords.Contains(w))
                .Select(w => w.ToLower())
                .ToArray()));
        }

        private Dictionary<string, int> GetFrequencyWords(IEnumerable<string> formattedWords)
        {
            return formattedWords
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());
        }

        private List<string> ExtractTextFromFile(string path)
        {
            var result = new List<string>();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                result.AddRange(line.Split(' ').Select(w => w.Trim(punctuationMarks)));
            }
            
            return result;
        }
    }
}