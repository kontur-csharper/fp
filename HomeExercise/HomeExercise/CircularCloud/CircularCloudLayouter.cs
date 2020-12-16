using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;

namespace HomeExercise
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        public Point Center { get; }
        private readonly List<Rectangle> rectanglesInCloud = new List<Rectangle>();
        private readonly ISpiral spiral;

        public CircularCloudLayouter(ISpiral spiral)
        {
            Center = spiral.Center;
            this.spiral = spiral;
        }
        
        private bool IsIntersect(Rectangle currentRectangle)
        {
            return rectanglesInCloud.Any(currentRectangle.IntersectsWith);
        }
        
        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            var sizeValidity = CheckSizeValidity(rectangleSize).OnFail(Console.WriteLine);
            
            return sizeValidity.IsSuccess
                ? Result.Of(() => GetRectangle(rectangleSize)).OnFail(Console.WriteLine)
                : Result.Fail<Rectangle>(sizeValidity.Error);
        }

        private Rectangle GetRectangle(Size rectangleSize)
        {
            while (true)
            {
                var rectangleLocation = spiral.GetNextPoint();
                var currentRectangle = new Rectangle(rectangleLocation, rectangleSize);
                if (IsIntersect(currentRectangle)) continue;
                rectanglesInCloud.Add(currentRectangle);
                return currentRectangle;
            }
        }

        private Result<None> CheckSizeValidity(Size size)
        {
            return size.Height >= 0 && size.Width >= 0 
                ? Result.Ok() 
                : Result.Fail<None>("Incorrect rectangle size");
        }
    }
}