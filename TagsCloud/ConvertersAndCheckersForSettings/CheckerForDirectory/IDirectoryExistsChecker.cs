using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.CheckerForDirectory
{
    public interface IDirectoryChecker
    {
        Result<string> GetExistingDirectory(string path);
    }
}