using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    internal class CircularCloudLayouter : ICloudLayouter
    {
        private const int RollBackPixelsCount = 10; // Оптимальное значение величины отката радиуса спирали в пикселях
        private bool buildingStarted;
        private Spiral spiral;
        private int imageSize;
        public IReadOnlyCollection<Rectangle> Rectangles { get; set; }

        public CircularCloudLayouter()
        {
            Reset();
        }

        public void Reset()
        {
            Rectangles = new List<Rectangle>();
            imageSize = 200;
            spiral = new Spiral(new Point(imageSize/2, imageSize/2), this);
            buildingStarted = false;
        }

        public Result<None> ChangeCenter(Point newCenter)
        {
            if (buildingStarted)
            {
                var offset = new Point(newCenter.X - spiral.Center.X, newCenter.Y - spiral.Center.Y);
                foreach (var rectangle in Rectangles)
                {
                    rectangle.Offset(offset);
                    if (CheckOutOfBounds(rectangle))
                        return Result.Fail<None>("Облако вышло за границы изображения");
                }
            }
            spiral = new Spiral(newCenter, this);
            imageSize = newCenter.X * 2;
            return Result.Ok();
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            buildingStarted = true;
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                return Result.Fail<Rectangle>($"Invalid rectangleSize = ({rectangleSize.Width}, {rectangleSize.Height})");
            spiral.RollBackRadius(RollBackPixelsCount);
            while (true)
            {
                var currentCoordinate = spiral.GetNextPosition(rectangleSize);
                var currentRectangle = new Rectangle(currentCoordinate, rectangleSize);

                if (CheckOutOfBounds(currentRectangle))
                    return Result.Fail<Rectangle>("Облако вышло за границы изображения");

                if (!CheckIntersections(currentRectangle))
                {
                    spiral.ShiftRectangle(ref currentRectangle);
                    ((List<Rectangle>)Rectangles).Add(currentRectangle);
                    return Result.Ok(currentRectangle);
                }
            }
        }

        private bool CheckOutOfBounds(Rectangle currentRectangle)
        {
            return currentRectangle.Left < 0 || currentRectangle.Right > imageSize
                                             || currentRectangle.Top < 0 
                                             || currentRectangle.Bottom > imageSize;
        }

        public bool CheckIntersections(Rectangle rectangle)
        {
            return Rectangles.Any(otherRectangle => rectangle.IntersectsWith(otherRectangle));
        }
    }
}