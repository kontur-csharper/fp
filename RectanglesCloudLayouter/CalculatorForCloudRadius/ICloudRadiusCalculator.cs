using System.Drawing;

namespace RectanglesCloudLayouter.CalculatorForCloudRadius
{
    public interface ICloudRadiusCalculator
    {
        int CloudRadius { get; }
        void UpdateCloudRadius(Point spiralCenter, Rectangle currentRectangle);
    }
}