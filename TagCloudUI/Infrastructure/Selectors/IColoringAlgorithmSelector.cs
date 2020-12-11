using TagCloud.Core;
using TagCloud.Core.ColoringAlgorithms;

namespace TagCloudUI.Infrastructure.Selectors
{
    public interface IColoringAlgorithmSelector
    {
        Result<IColoringAlgorithm> GetAlgorithm(ColoringTheme theme);
    }
}