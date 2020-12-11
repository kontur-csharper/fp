using TagCloud.Core.ColoringAlgorithms;
using TagCloud.Core.LayoutAlgorithms;

namespace TagCloudUI.Infrastructure
{
    public interface IAppSettings : ITagCloudSettings
    {
        string OutputPath { get; }
        string ImageFormat { get; }
    }
}