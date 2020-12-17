using System.Drawing;
using TagCloud.ExceptionHandler;
using TagCloud.Visualizer;

namespace TagCloud.ImageSaver
{
    public interface IImageSaver
    {
        public Result<None> SaveImage(Result<Bitmap> bitmapResult, string savePath, ImageOptions imageOptions);
    }
}