using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class CloudLayouter : ILayouter
    {
        public CloudLayouter(ISpiral spiral)
        {
            Spiral = spiral;
            Center = spiral.Center;
            Rectangles = new List<Rectangle>();
        }

        private Point Center { get; }
        private ISpiral Spiral { get; }
        public List<Rectangle> Rectangles { get; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            return Result.Ok(rectangleSize)
                .Then(ValidateRectangleHeight)
                .Then(ValidateRectangleWidth)
                .OnFail(e => throw new ArgumentException(e, nameof(rectangleSize)))
                .SelectMany(x => Result.Ok(GetNewRectangle(x)))
                .GetValueOrThrow();
        }

        private Rectangle GetNewRectangle(Size rectangleSize)
        {
            Rectangle rectangle;

            do
            {
                var location = Spiral.GetNextPoint();
                rectangle = new Rectangle(location, rectangleSize);
            } while (Collided(rectangle));

            rectangle = MoveCloserToCenter(rectangle);
            Rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle MoveCloserToCenter(Rectangle rectangle)
        {
            var movedRectangle = rectangle;

            while (!Collided(rectangle) &&
                   rectangle.X != Center.X &&
                   rectangle.Y != Center.Y)
            {
                movedRectangle = rectangle;
                var deltaX = Center.X - rectangle.X < 0 ? -1 : 1;
                var deltaY = Center.Y - rectangle.Y < 0 ? -1 : 1;

                var position = new Point(rectangle.X + deltaX, rectangle.Y + deltaY);
                rectangle = new Rectangle(position, rectangle.Size);
            }

            return movedRectangle;
        }

        private bool Collided(Rectangle newRectangle)
        {
            return Rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle));
        }

        private Result<Size> ValidateRectangleHeight(Size size)
        {
            return Validate(size, x => x.Height <= 0, $"Rectangle height should be positive, but was: {size.Height}");
        }

        private Result<Size> ValidateRectangleWidth(Size size)
        {
            return Validate(size, x => x.Width <= 0, $"Rectangle width should be positive, but was: {size.Width}");
        }

        private Result<T> Validate<T>(T obj, Func<T, bool> predicate, string exception)
        {
            return predicate(obj)
                ? Result.Fail<T>(exception)
                : Result.Ok(obj);
        }
    }
}