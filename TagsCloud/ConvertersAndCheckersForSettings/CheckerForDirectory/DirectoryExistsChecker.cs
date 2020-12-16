using System.IO;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.CheckerForDirectory
{
    public class DirectoryExistsChecker : IDirectoryChecker
    {
        public Result<string> GetExistingDirectory(string path)
        {
            var directoryLength = path.LastIndexOf(Path.DirectorySeparatorChar);
            if (directoryLength == -1 || Directory.Exists(path.Substring(0, directoryLength)))
                return new Result<string>(null, path);
            return new Result<string>("Doesn't contain the directory to save file");
        }
    }
}