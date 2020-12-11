using TagCloud.Core.ColoringAlgorithms;
using TagCloud.Core.LayoutAlgorithms;

namespace TagCloudUI.Infrastructure
{
    public interface ITagCloudSettings
    {
        string InputPath { get; }
        int ImageWidth { get; }
        int ImageHeight { get; }
        LayoutAlgorithmType LayoutAlgorithmType { get; }
        ColoringTheme ColoringTheme { get; }
        string FontName { get; }
        int WordsCount { get; }
    }
}