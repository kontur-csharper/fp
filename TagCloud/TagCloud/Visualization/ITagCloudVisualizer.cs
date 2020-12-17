using System.Drawing;
using TagCloud.ErrorHandling;

namespace TagCloud.Visualization
{
    public interface ITagCloudVisualizer
    {
        Result<Bitmap> CreateTagCloudBitMap(int width, int height, Color[] colors, string fontFamily);
    }
}