﻿using System.Drawing;
using System.Linq;
using TagCloud.Visualization.WordsColorings;
using ResultOf;

namespace TagCloud.Visualization
{
    internal class VisualizationInfo
    {
        private readonly Size? size;
        private readonly string font;
        private readonly IWordsColoring wordsColoring;

        internal VisualizationInfo(IWordsColoring coloring, Size? size = null, string font = null)
        {
            this.size = size;
            this.font = Fonts.GetFont(font);
            wordsColoring = coloring;
        }

        internal static Result<Size?> ReadSize(string sizeStr)
        {
            var error = new Result<Size?>("Was incorrect size, must be two numbers like \"2000 1500\" or \"dynamic\"");
            if (sizeStr == "dynamic")
            {
                return new Result<Size?>(null, null);
            }
            try
            {
                var result = ParseString(sizeStr);
                if (result.Length != 2)
                    return error;
                if (result.Any(i => i < 0))
                    return error;
                if (result[0] == 0 || result[1] == 0)
                    return new Result<Size?>("must be two positive number, but was negative");
                return new Result<Size?>(null, new Size(result[0], result[1]));
            }
            catch
            {
                return error;
            }
        }

        public static bool FontIsCorrect(string font) => FontFamily.Families.Select(f => f.Name).Contains(font);

        private static int[] ParseString(string str) =>
            str.Split(' ')
            .Where(s => s != string.Empty)
            .Select(s => int.Parse(s))
            .ToArray();

        internal bool TryGetSize(out Size size)
        {
            size = this.size ?? Size.Empty;
            return this.size != null;
        }

        internal Font GetFont(int emSize) => new Font(font, emSize);

        internal Color GetColor(string word, Rectangle location, TagCloud cloud) =>
            wordsColoring.GetColor(word, location, cloud);
        
        internal SolidBrush GetSolidBrush(string word, Rectangle location, TagCloud cloud) => 
            new SolidBrush(GetColor(word, location, cloud));
    }
}
