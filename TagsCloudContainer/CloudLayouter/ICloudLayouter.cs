using System;
using System.Drawing;

namespace TagsCloudContainer.CloudLayouter
{
    public interface ICloudLayouter : IDisposable
    {
        Result<Rectangle> PutNextRectangle(Size rectangleSize);
    }
}