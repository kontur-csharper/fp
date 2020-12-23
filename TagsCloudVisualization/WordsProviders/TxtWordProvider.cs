using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization.WordsProviders
{
    public class TxtWordProvider : IWordProvider
    {
        public Result<List<string>> GetWords(string filepath)
        {
            return ValidateFile(filepath)
                .Then(ReadWords);
        }

        private static Result<List<string>> ReadWords(string filepath)
        {
            var words = new List<string>();

            using var sr = new StreamReader(filepath);
            string word;
            while ((word = sr.ReadLine()) != null) words.Add(word);

            return words;
        }

        private static Result<string> ValidateFile(string filepath)
        {
            return !File.Exists(filepath)
                ? Result.Fail<string>("File with words was not found")
                : Result.Ok(filepath);
        }
    }
}