using System.IO;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.CheckerForFile
{
    public class FileExistsСhecker : IFileExistsСhecker
    {
        public Result<string> GetProvenPath(string pathToFile)
        {
            return File.Exists(pathToFile)
                ? new Result<string>(null, pathToFile)
                : new Result<string>("Doesn't contain the text file");
        }
    }
}