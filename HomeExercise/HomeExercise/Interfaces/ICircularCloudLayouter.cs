using System.Drawing;
using ResultOf;

namespace HomeExercise
{
    public interface ICircularCloudLayouter
    {
        Point Center { get; }
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}