using System.Collections.Generic;
using System.Linq;
using TagCloud.Core;
using TagCloud.Core.LayoutAlgorithms;

namespace TagCloudUI.Infrastructure.Selectors
{
    public class LayoutAlgorithmSelector : ILayoutAlgorithmSelector
    {
        private readonly Dictionary<LayoutAlgorithmType, ILayoutAlgorithm> typeToAlgorithm;

        public LayoutAlgorithmSelector(IEnumerable<ILayoutAlgorithm> algorithms)
        {
            typeToAlgorithm = algorithms.ToDictionary(algorithm => algorithm.Type);
        }

        public Result<ILayoutAlgorithm> GetAlgorithm(LayoutAlgorithmType type)
        {
            return typeToAlgorithm.TryGetValue(type, out var algorithm)
                ? algorithm.AsResult()
                : Result.Fail<ILayoutAlgorithm>($"There is no such algorithm: {type}");
        }
    }
}