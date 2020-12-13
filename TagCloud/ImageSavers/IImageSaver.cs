using System.Drawing;

namespace TagCloud.ImageSavers
{
    public interface IImageSaver
    {
        public Result<None> Save(Bitmap bitmap);
    }
}