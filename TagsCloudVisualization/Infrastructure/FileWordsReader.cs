using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloudCreating.Infrastructure;
using TagsCloudVisualization.Contracts;

namespace TagsCloudVisualization.Infrastructure
{
    public class FileWordsReader : IWordsReader
    {
        public Result<IEnumerable<string>> GetAllData(string fileName) =>
            fileName.AsResult()
                .Then(File.ReadAllText)
                .Then(words => Regex.Split(words, @"\W+").AsEnumerable())
                .ReplaceError(err => "File is not exist");
    }
}