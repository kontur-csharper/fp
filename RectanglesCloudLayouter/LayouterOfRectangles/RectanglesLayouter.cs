using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RectanglesCloudLayouter.CalculatorForCloudRadius;
using RectanglesCloudLayouter.SpecialMethods;
using RectanglesCloudLayouter.SpiralAlgorithm;

namespace RectanglesCloudLayouter.LayouterOfRectangles
{
    public class RectanglesLayouter : IRectanglesLayouter
    {
        private readonly List<Rectangle> _rectangles;
        private readonly ISpiral _spiral;
        private readonly ICloudRadiusCalculator _cloudRadiusCalculator;

        public RectanglesLayouter(ISpiral spiral, ICloudRadiusCalculator cloudRadiusCalculator)
        {
            _rectangles = new List<Rectangle>();
            _spiral = spiral;
            _cloudRadiusCalculator = cloudRadiusCalculator;
        }

        public Rectangle GetCurrentRectangle => _rectangles.Last();

        public int RectanglesCount => _rectangles.Count;

        public int CloudRadius => _cloudRadiusCalculator.CloudRadius;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(_spiral.GetNewSpiralPoint() - rectangleSize / 2, rectangleSize);
            while (RectanglesIntersection.IsAnyIntersectWithRectangles(rectangle, _rectangles))
                rectangle = new Rectangle(_spiral.GetNewSpiralPoint() - rectangleSize / 2, rectangleSize);
            _rectangles.Add(rectangle);
            _cloudRadiusCalculator.UpdateCloudRadius(_spiral.Center, rectangle);
            return rectangle;
        }
    }
}