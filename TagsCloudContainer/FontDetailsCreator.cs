﻿namespace TagsCloudContainer
{
    public class FontDetailsCreator : IFontDetailsCreator
    {
        private readonly string fontName;
        public readonly int MinFontSize = 8;
        public readonly int MaxFontSize = 35;

        public FontDetailsCreator(string fontName)
        {
            this.fontName = fontName;
        }

        public string GetFontName(int wordsCount, int maxWordsCount)
        {
            return fontName;
        }

        public float GetFontSize(int wordsCount, int maxWordsCount)
        {
            var size = (float)wordsCount / maxWordsCount * (MaxFontSize - MinFontSize) + MinFontSize;
            return size;
        }
    }
}