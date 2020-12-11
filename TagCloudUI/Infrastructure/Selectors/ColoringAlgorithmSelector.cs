using System.Collections.Generic;
using System.Linq;
using TagCloud.Core;
using TagCloud.Core.ColoringAlgorithms;

namespace TagCloudUI.Infrastructure.Selectors
{
    public class ColoringAlgorithmSelector : IColoringAlgorithmSelector
    {
        private readonly Dictionary<ColoringTheme, IColoringAlgorithm> themeToAlgorithm;

        public ColoringAlgorithmSelector(IEnumerable<IColoringAlgorithm> algorithms)
        {
            themeToAlgorithm = algorithms.ToDictionary(algorithm => algorithm.Theme);
        }

        public Result<IColoringAlgorithm> GetAlgorithm(ColoringTheme theme)
        {
            return themeToAlgorithm.TryGetValue(theme, out var algorithm)
                ? algorithm.AsResult()
                : Result.Fail<IColoringAlgorithm>($"There is no such coloring theme: {theme}");
        }
    }
}