using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.CheckerForFile
{
    public interface IFileExistsСhecker
    {
        Result<string> GetProvenPath(string pathToFile);
    }
}