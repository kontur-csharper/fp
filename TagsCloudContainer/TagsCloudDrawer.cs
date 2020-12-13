using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class TagsCloudDrawer : ITagsCloudDrawer
    {
        private readonly IDrawSettings settings;
        private readonly ICloudLayouter layouter;
        private readonly IFontDetailsCreator fontDetailsCreator;
        private readonly IFontColorCreator colorCreator;
        private readonly List<Font> fontsCache = new List<Font>();
        private readonly SolidBrush brush = new SolidBrush(Color.Black);

        public TagsCloudDrawer(IDrawSettings settings, ICloudLayouter layouter,
            IFontDetailsCreator fontDetailsCreator, IFontColorCreator colorCreator)
        {
            this.settings = settings;
            this.layouter = layouter;
            this.fontDetailsCreator = fontDetailsCreator;
            this.colorCreator = colorCreator;
        }

        public Bitmap Draw(Dictionary<string, int> countedWords)
        {
            var bitmap = new Bitmap(settings.ImageWidth, settings.ImageHeight);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(settings.BackgroundColor);
            var maxWordsCount = countedWords.Select(pair => pair.Value).Max();

            foreach (var (word, wordsCount) in countedWords)
            {
                DrawWord(word, wordsCount, maxWordsCount, graphics);
            }

            layouter.Reset();

            return bitmap;
        }

        private void DrawWord(string word, int count, int maxWordsCount, Graphics graphics)
        {
            var font = GetFont(count, maxWordsCount);
            brush.Color = colorCreator.GetFontColor(count, maxWordsCount);
            var rectSize = Size.Ceiling(graphics.MeasureString(word, font));
            var wordRect = layouter.PutNextRectangle(rectSize);

            graphics.DrawString(word, font, brush, wordRect);
        }

        private Font GetFont(int count, int maxWordsCount)
        {
            var fontName = fontDetailsCreator.GetFontName(count, maxWordsCount);
            var size = fontDetailsCreator.GetFontSize(count, maxWordsCount);
            var font = fontsCache.FirstOrDefault(font =>
                font.Name == fontName && font.Size == size);

            if (font != null)
                return font;

            font = new Font(fontName, size);
            fontsCache.Add(font);
            return font;
        }

        public void Dispose()
        {
            foreach (var font in fontsCache)
            {
                font.Dispose();
            }

            brush.Dispose();
        }
    }
}