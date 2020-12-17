using System.IO;
using TagCloud.ErrorHandling;

namespace TagCloud.WordsProvider
{
    public static class FileWordsProviderFactory
    {
        public static Result<IWordsProvider> Create(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            switch (extension)
            {
                case ".txt":
                    return TxtWordsProvider.Create(filePath);
                case ".doc":
                case ".docx":
                    return MicrosoftWordWordsProvider.Create(filePath);
                default:
                    return Result.Fail<IWordsProvider>($"Extension {extension} is not supported");
            }
        }
    }
}