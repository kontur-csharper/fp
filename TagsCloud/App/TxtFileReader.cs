using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public class TxtFileReader : FileReader
    {
        public override HashSet<string> AvailableFileTypes { get; } = new HashSet<string> {"txt"};

        protected override Result<string[]> ReadWordsInternal(string fileName)
        {
            return Result
                .Of(() => File.ReadAllLines(fileName).SelectMany(line => splitRegex.Split(line)).ToArray());
        }
    }
}