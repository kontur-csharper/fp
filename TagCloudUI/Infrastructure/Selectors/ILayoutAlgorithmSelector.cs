using TagCloud.Core;
using TagCloud.Core.LayoutAlgorithms;

namespace TagCloudUI.Infrastructure.Selectors
{
    public interface ILayoutAlgorithmSelector
    {
        Result<ILayoutAlgorithm> GetAlgorithm(LayoutAlgorithmType type);
    }
}