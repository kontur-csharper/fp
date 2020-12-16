using System.Drawing;

namespace RectanglesCloudLayouter.SpiralAlgorithm
{
    public interface ISpiral
    {
        Point Center { get; }
        Point GetNewSpiralPoint();
    }
}