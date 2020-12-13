using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Layouters;
using TagCloud.Settings;

namespace TagCloud.Drawers
{
    public class TagDrawer : IDisposable, ITagDrawer
    {
        private readonly IRectangleLayouter layouter;
        private readonly DrawerSettings settings;
        
        private Graphics graphics;
        private Bitmap bitmap;

        private Random random = new Random();
        
        public TagDrawer(DrawerSettings settings, IRectangleLayouter layouter)
        {
            this.layouter = layouter;
            this.settings = settings;
        }

        public Result<Bitmap> DrawTagCloud(IReadOnlyCollection<TagInfo> tags)
        {
            bitmap = new Bitmap(settings.ImageSize.Width, settings.ImageSize.Height);
            graphics = Graphics.FromImage(bitmap);
            SetBackGroundColor(settings.ImageSize, settings.BackgroundColor);
            
            if (settings.OrderByWeight)
                tags = tags.OrderByDescending(t => t.Weight).ToArray();

            var drawingResult = Result.Ok();
            drawingResult = tags.Aggregate(drawingResult, (current, tag) => current.Then(_ => DrawTag(tag)));

            return drawingResult.Then(_ => bitmap);
        }

        private Result<None> DrawTag(TagInfo tag)
        {
            return GetFont(tag).Then(font => PlaceAndDrawTag(tag, font));
        }

        private Result<None> PlaceAndDrawTag(TagInfo tag, Font font)
        {
            return layouter
                .PutNextRectangle(MeasureStringSize(tag.Value, font))
                .Then(FitRectangleInBorders)
                .Then(rectangle => 
                {
                    FillRectangle(rectangle, settings.ForegroundColor);
                    DrawString(tag.Value, font, rectangle.Location, GetFontColor());
                });
        }

        private Result<Rectangle> FitRectangleInBorders(Rectangle rectangle)
        {
            if (rectangle.X < 0 
                || rectangle.Y < 0 
                || rectangle.X + rectangle.Width >= settings.ImageSize.Width 
                || rectangle.Y + rectangle.Height >= settings.ImageSize.Height)
                return Result.Fail<Rectangle>("Tag cloud didn't fit on the image.\n" +
                                              "Try increasing the image size or decreasing the font size");
            return Result.Ok(rectangle);
        }
        
        private Result<Font> GetFont(TagInfo tag)
        {
            var fontSize = (int)((settings.MaxFontSize - settings.MinFontSize) * tag.Weight + settings.MinFontSize);
            var font = new Font(settings.FontFamily, fontSize);
            return font.FontFamily.Name != settings.FontFamily 
                ? Result.Fail<Font>("Font wasn't found on the system.") 
                : font;
        }

        private Size MeasureStringSize(string text, Font font)
        {
            return graphics.MeasureString(text, font).ToSize();
        }

        private void SetBackGroundColor(Size pictureSize, Color backgroundColor)
        {
            FillRectangle(new Rectangle(new Point(0, 0), pictureSize), backgroundColor);
        }
        
        private void FillRectangle(Rectangle rectangle, Color? color = null)
        {
            using var brush = new SolidBrush(GetColor(color));
            graphics.FillRectangle(brush, rectangle);
        }
        
        private void DrawString(string text, Font font, Point textPosition, Color? color = null)
        {
            using var brush = new SolidBrush(GetColor(color));
            graphics.DrawString(text, font, brush, textPosition);
        }
        
        private Color GetFontColor()
        {
            if (settings.Colors.Count == 0)
                return GetRandomColor();
            return settings.Colors[random.Next(0, settings.Colors.Count)];
        }

        private Color GetColor(Color? color = null)
        {
            return color ?? GetRandomColor();
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(
                random.Next(0, 256),
                random.Next(0, 256),
                random.Next(0, 256)
            );
        }

        public void Dispose()
        {
            bitmap?.Dispose();
            graphics?.Dispose();
        }
    }
}