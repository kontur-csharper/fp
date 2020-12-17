using System.Drawing;
using System.IO;
using TagCloud.ExceptionHandler;
using TagCloud.Visualizer;

namespace TagCloud.ImageSaver
{
    public class PngSaver : IImageSaver
    {
        public Result<None> SaveImage(Result<Bitmap> bitmapResult, string savePath, ImageOptions imageOptions)
        {
            if (Path.GetExtension(imageOptions.ImageSaveName) != ".png")
            {
                return Result.Fail<None>("Расширение файла для сохранения не является поддерживаемым");
            }

            bitmapResult.GetValueOrThrow().Save(Path.Combine(savePath, $"{imageOptions.ImageSaveName}"));
            return new Result<None>(null);
        }
    }
}