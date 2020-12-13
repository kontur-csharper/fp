using System.IO;

namespace TagsCloudContainer
{
    public class TxtFileWordsLoader : IWordsLoader
    {
        protected readonly string pathToFile;

        public TxtFileWordsLoader(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }

        public string[] GetWords()
        {
            return File.ReadAllLines(pathToFile);
        }
    }
}