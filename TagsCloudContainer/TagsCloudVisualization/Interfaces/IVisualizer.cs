using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.Interfaces
{
    public interface IVisualizer
    {
        Bitmap GetBitmap(List<Rectangle> rectangles);
    }
}