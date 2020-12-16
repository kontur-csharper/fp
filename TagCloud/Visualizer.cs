using System;
using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagCloud.BackgroundPainter;

namespace TagCloud
{
    public class Visualizer: IVisualizer
    {
        private readonly ICanvas canvas;
        private readonly IPathCreator creator;
        private readonly ITagsCreator tagsCreator;
        private readonly IBackgroundPainter backgroundPainter;
        private const double fontCoefficient = 0.6;
        
        public Visualizer(ICanvas canvas, IPathCreator pathCreator, ITagsCreator tagsCreator, IBackgroundPainter backgroundPainter)
        {
            this.canvas = canvas;
            creator = pathCreator;
            this.tagsCreator = tagsCreator;
            this.backgroundPainter = backgroundPainter;
        }

        public Result<string> Visualize(string filename, FontFamily fontFamily, Color stringColor)
        {
            var getTagsResult = tagsCreator.GetTags(filename, canvas.Height);

            return getTagsResult.Then(tags => DrawAndSaveTags(tags, fontFamily, stringColor));
        }

        private string DrawAndSaveTags(List<Tuple<string, Rectangle>> tags, FontFamily fontFamily, Color stringColor)
        {
            var path = creator.GetNewPngPath();
            using (var bitmap = new Bitmap(canvas.Width, canvas.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                backgroundPainter.Draw(tags, canvas, graphics);
                DrawAllStrings(tags, fontFamily, stringColor, graphics);
                
                bitmap.Save(path);
            }
            return path;
        }
        
        private static void DrawAllStrings(List<Tuple<string, Rectangle>> tags, FontFamily fontFamily, Color color, Graphics graphics)
        {
            using (var textBrush = new SolidBrush(color))
            {
                foreach (var (str, rectangle) in tags)
                {
                    DrawString(str, rectangle, fontFamily, textBrush, graphics);
                }
            }
        }

        private static void DrawString(string str, Rectangle rectangle, FontFamily fontFamily, Brush textBrush, Graphics graphics)
        {
            var x = rectangle.X;
            var y = rectangle.Y;
            var fontSize = (int)Math.Round(rectangle.Height * fontCoefficient);
            if (rectangle.Height < 2)
                return;
            using (var font = new Font(fontFamily, fontSize))
            {
                graphics.DrawString(str, font, textBrush, x, y);
            }
        }
    }
}