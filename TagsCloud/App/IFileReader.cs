using System.Collections.Generic;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public interface IFileReader
    {
        HashSet<string> AvailableFileTypes { get; }
        Result<string[]> ReadWords(string fileName);
    }
}