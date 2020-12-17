using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.ExceptionHandler;
using TagCloud.Visualizer;

namespace TagCloud.ImageSaver
{
    public class CompositeImageSaver : IImageSaver
    {
        private readonly IEnumerable<IImageSaver> implementations;

        public CompositeImageSaver(IEnumerable<IImageSaver> implementations)
        {
            this.implementations = implementations;
        }

        public Result<None> SaveImage(Result<Bitmap> bitmapResult, string savePath, ImageOptions imageOptions)
        {
            var isImplementationFound = implementations.Any(imageSaver =>
                imageSaver.SaveImage(bitmapResult.GetValueOrThrow(), savePath, imageOptions).IsSuccess);
            bitmapResult.GetValueOrThrow().Dispose();
            return isImplementationFound
                ? new Result<None>(null)
                : Result.Fail<None>("Расширение файла для сохранения не является поддерживаемым");
        }
    }
}