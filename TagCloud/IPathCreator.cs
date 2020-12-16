namespace TagCloud
{
    public interface IPathCreator
    {
        string GetCurrentPath();

        string GetNewPngPath();

        string GetPathToFile(string fileName);
    }
}