using System.Drawing;
using TagsCloud.Infrastructure;

namespace TagsCloud.Layouters
{
    public interface ICloudLayouter
    {
        void UpdateCenterPoint(ImageSettings settings);
        void ClearLayouter();
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}