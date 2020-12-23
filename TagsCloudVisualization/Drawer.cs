using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudTags;
using TagsCloudVisualization.Configs;

namespace TagsCloudVisualization
{
    public static class Drawer
    {
        public static Result<Bitmap> DrawImage(List<ICloudTag> rectangles, IConfig config)
        {
            return CheckPoint(config)
                .Then(y => CheckRectangles(rectangles)
                    .Then(x => DrawRectangles(x, y)));
        }

        private static Result<Bitmap> DrawRectangles(List<ICloudTag> rectangles, IConfig config)
        {
            var actualSize = new Size(config.Center.X + GetDeltaX(rectangles),
                config.Center.Y + GetDeltaY(rectangles));
            var size = new Size(Math.Max(actualSize.Width, config.ImageSize.Width),
                Math.Max(actualSize.Height, config.ImageSize.Height));
            var image = new Bitmap(size.Width, size.Height);
            using var graphics = Graphics.FromImage(image);

            foreach (var rectangle in rectangles)
            {
                graphics.DrawString(rectangle.Text, config.Font, new SolidBrush(config.TextColor),
                    rectangle.Rectangle);
                graphics.DrawRectangle(new Pen(config.TextColor), rectangle.Rectangle);
            }

            return image;
        }

        private static Result<IConfig> CheckPoint(IConfig config)
        {
            return config.Center.X < 0 || config.Center.Y < 0
                ? Result.Fail<IConfig>("X or Y of center was negative")
                : Result.Ok(config);
        }

        private static Result<List<ICloudTag>> CheckRectangles(List<ICloudTag> rectangles)
        {
            return !rectangles.Any()
                ? Result.Fail<List<ICloudTag>>("The sequence contains no elements")
                : Result.Ok(rectangles);
        }

        private static int GetDeltaX(List<ICloudTag> rectangles)
        {
            return rectangles.Select(elem => elem.Rectangle.Right).Max();
        }

        private static int GetDeltaY(List<ICloudTag> rectangles)
        {
            return rectangles.Select(elem => elem.Rectangle.Bottom).Max();
        }
    }
}