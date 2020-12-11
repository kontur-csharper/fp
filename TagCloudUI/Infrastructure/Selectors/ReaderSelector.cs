using System.Collections.Generic;
using System.Linq;
using TagCloud.Core;
using TagCloud.Core.FileReaders;

namespace TagCloudUI.Infrastructure.Selectors
{
    public class ReaderSelector : IReaderSelector
    {
        private readonly Dictionary<FileExtension, IFileReader> extensionToReader;

        public ReaderSelector(IEnumerable<IFileReader> readers)
        {
            extensionToReader = readers.ToDictionary(reader => reader.Extension);
        }

        public Result<IFileReader> GetReader(FileExtension extension)
        {
            return extensionToReader.TryGetValue(extension, out var reader)
                ? reader.AsResult()
                : Result.Fail<IFileReader>($"Unable to read file with this extension: {extension}");
        }
    }
}