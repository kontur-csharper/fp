using TagCloud.Core;
using TagCloud.Core.FileReaders;

namespace TagCloudUI.Infrastructure.Selectors
{
    public interface IReaderSelector
    {
        Result<IFileReader> GetReader(FileExtension extension);
    }
}