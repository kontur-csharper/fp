using System.IO;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class ImageFilePathGenerator : IFilePathGenerator
    {
        public ImageFilePathGenerator(IDateTimeProvider dateTimeProvider)
        {
            Root = Directory.GetCurrentDirectory();
            DateTimeProvider = dateTimeProvider;
        }

        private string Root { get; }
        private IDateTimeProvider DateTimeProvider { get; }

        public string GetNewFilePath()
        {
            var dateTime = DateTimeProvider.GetDateTimeNow();
            return Path.Join(Root, $"{dateTime:MMddyy-HHmmssffffff}.png");
        }
    }
}