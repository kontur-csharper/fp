using System;
using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class RectangleVisualizer : IVisualizer
    {
        public Bitmap GetBitmap(List<Rectangle> rectangles)
        {
            var imageSize = GetImageSize(rectangles);
            var pen = new Pen(Color.MediumVioletRed, 4);

            var bitmap = new Bitmap(imageSize.Width + (int) pen.Width, imageSize.Height + (int) pen.Width);
            using var graphics = Graphics.FromImage(bitmap);

            if (rectangles.Count != 0) graphics.DrawRectangles(pen, rectangles.ToArray());

            return bitmap;
        }

        protected Size GetImageSize(IEnumerable<Rectangle> rectangles)
        {
            var width = 0;
            var height = 0;

            foreach (var rectangle in rectangles)
            {
                Result.Ok(rectangle)
                    .Then(ValidateRectangleLeft)
                    .Then(ValidateRectangleTop)
                    .OnFail(e => throw new ArgumentException(e, nameof(rectangle)));

                if (width < rectangle.Right) width = rectangle.Right;
                if (height < rectangle.Bottom) height = rectangle.Bottom;
            }

            return new Size(width, height);
        }

        private Result<Rectangle> ValidateRectangleTop(Rectangle rectangle)
        {
            return Validate(rectangle, x => x.Top < 0,
                $"Rectangle top coordinate out of image boundaries, and was: {rectangle.Top}");
        }

        private Result<Rectangle> ValidateRectangleLeft(Rectangle rectangle)
        {
            return Validate(rectangle, x => x.Left < 0,
                $"Rectangle left coordinate out of image boundaries, and was: {rectangle.Left}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}