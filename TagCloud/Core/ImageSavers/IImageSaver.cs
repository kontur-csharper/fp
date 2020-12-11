using System.Drawing;

namespace TagCloud.Core.ImageSavers
{
    public interface IImageSaver
    {
        Result<string> Save(Bitmap bitmap, string fullPath, string format);
    }
}