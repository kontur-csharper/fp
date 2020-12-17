using System.Collections.Generic;
using System.Drawing;
using TagCloud.ErrorHandling;

namespace TagCloud.ConsoleAppHelper
{
    public static class ColorsParser
    {
        private static readonly Dictionary<char, Color> colors = new Dictionary<char, Color>
        {
            {'r', Color.Red},
            {'g', Color.Green},
            {'b', Color.Blue},
            {'y', Color.Yellow},
            {'p', Color.Purple}
        };

        public static Result<Color[]> ParseColors(string input)
        {
            var result = new List<Color>();
            foreach (var colorChar in input)
            {
                if (!colors.ContainsKey(colorChar))
                    return new Result<Color[]>($"Color {colorChar} is not supported");

                result.Add(colors[colorChar]);
            }

            return result.ToArray();
        }
    }
}