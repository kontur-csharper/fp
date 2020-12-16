using ResultPattern;
using TagsCloud.Reader;
using TagsCloud.Saver;
using TagsCloud.TagsCloudVisualization;
using TagsCloud.TagsLayouter;

namespace TagsCloud.ProcessorOfApp
{
    public class AppProcessor : IAppProcessor
    {
        private readonly IVisualization _visualization;
        private readonly IWordTagsLayouter _wordTagsLayouter;
        private readonly IFileReader _fileReader;
        private readonly IImageSaver _imageSaver;

        public AppProcessor(IVisualization visualization,
            IWordTagsLayouter wordTagsLayouter,
            IFileReader fileReader,
            IImageSaver imageSaver)
        {
            _visualization = visualization;
            _wordTagsLayouter = wordTagsLayouter;
            _fileReader = fileReader;
            _imageSaver = imageSaver;
        }

        public Result<None> Run()
        {
            return _fileReader
                .GetTextFromFile()
                .Then(text => _wordTagsLayouter.GetWordTagsAndCloudRadius(text))
                .Then(tagsAndCloudRadius =>
                {
                    var (tags, cloudRadius) = tagsAndCloudRadius;
                    return _visualization.GetImageCloud(tags, cloudRadius);
                })
                .Then(image => _imageSaver.Save(image));
        }
    }
}