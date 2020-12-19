using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System;
using ResultOf;

namespace TagCloud.Visualization.WordsColorings
{
    public static class WordsColoringAssosiation
    {
        private static readonly Color red = Color.FromArgb(255, 255, 0, 0);
        private static readonly Color green = Color.FromArgb(255, 0, 255, 0);
        private static readonly Color blue = Color.FromArgb(255, 0, 0, 255);
        private static readonly Color black = Color.FromArgb(255, 255, 255, 255);
        private static readonly Random random = new Random();
        private static readonly Dictionary<string, IWordsColoring> colorings =
            new Dictionary<string, IWordsColoring>
            {
                ["red"] = new WordsColoringConst(red),
                ["green"] = new WordsColoringConst(green),
                ["blue"] = new WordsColoringConst(blue),
                ["black"] = new WordsColoringConst(black),
                ["random"] = new WordsColoringConst(GetRandomColor()),
                ["multi"] = new WordsColoringRandom(),
                ["line red"] = new WordsColoringLineBringhtness(red),
                ["line green"] = new WordsColoringLineBringhtness(green),
                ["line blue"] = new WordsColoringLineBringhtness(blue),
                ["line random"] = new WordsColoringLineBringhtness(GetRandomColor())
            };

        public static readonly HashSet<string> coloringNames = colorings.Keys.ToHashSet();

        private static Color GetRandomColor() => Color.FromArgb(255, random.Next(255), random.Next(255), random.Next(255));

        internal static Result<IWordsColoring> GetColoring(string name) 
        {
            if (!colorings.ContainsKey(name))
            {
                return new Result<IWordsColoring>($"doesn't have coloring with name \"{name}\"\n" +
                    $"List of coloring names:\n{string.Join('\n', coloringNames)}");
            }
            return new Result<IWordsColoring>(null, colorings[name]);
        }
    }
}
