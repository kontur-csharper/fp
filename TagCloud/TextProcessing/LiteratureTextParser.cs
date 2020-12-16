using System;
using System.IO;
using System.Linq;
using ResultOf;
using WeCantSpell.Hunspell;

namespace TagCloud.TextProcessing
{
    public class LiteratureTextParser : IWordParser
    {
        private readonly IPathCreator creator;
        private readonly ITextReader reader;
        private static readonly char[] separators = {' ', '.', ',', ':', '!', '?'};
        private const int minWordLength = 3;
        private static string[] unneccesaryWords = 
            {"мочь", "этот", "когда", "чтобы", "даже", "между", "если", "несколько", "который", "какой", "только",
                "очень", "более", "ничто", "кто", "он", "такой", "однако", "либо", "оный", "такой", "него"};
        
            
        public LiteratureTextParser(IPathCreator creator, ITextReader reader)
        {
            this.creator = creator;
            this.reader = reader;
        }
        
        public Result<string[]> GetWords(string inputFileName)
        {
            var path = creator.GetCurrentPath();
            return GetDictionary(path)
                    .Then(dict => GetNormalizeWords(dict, path + inputFileName));
        }

        private Result<string[]> GetNormalizeWords(WordList dictionary, string pathToFile)
        {
            return reader.ReadStrings(pathToFile)
                .Then(lines => NormalizeWords(lines, dictionary));
        }

        private static string[] NormalizeWords(string[] lines, WordList dictionary)
        {
            return lines
                .SelectMany(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                .Select(word => word.ToLower())
                .Select(word => GetRootForWord(word, dictionary))
                .Where(word => !(word is null))
                .Where(word => word.Length > minWordLength)
                .Where(word => !unneccesaryWords.Contains(word))
                .ToArray();
        }

        private static string GetRootForWord(string word, WordList dictionary)
        {
            return dictionary.ContainsEntriesForRootWord(word) ? word : dictionary.CheckDetails(word).Root;
        }

        private static Result<WordList> GetDictionary(string path)
        {
            if (!File.Exists(path + "ru_RU.dic") || !File.Exists(path + "ru_RU.aff"))
                return Result.Fail<WordList>("Not found dictionaries (ru_RU.dic/ru_RU.aff) by path " + path);
            try
            {
                return WordList.CreateFromFiles(path + "ru_RU.dic", path + "ru_RU.aff");
            }
            catch (Exception e)
            {
                return Result.Fail<WordList>("Unhandled exception: " + e);
            }
        }
    }
}