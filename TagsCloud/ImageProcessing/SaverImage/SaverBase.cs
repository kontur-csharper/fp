using System.Drawing;
using System.Drawing.Imaging;
using TagsCloud.ImageProcessing.SaverImage.ImageSavers;
using TagsCloud.ResultOf;

namespace TagsCloud.ImageProcessing.SaverImage
{
    public abstract class SaverBase : IImageSaver
    {
        public abstract ImageFormat ImageFormat { get; }

        public abstract bool CanSave(string path);

        public Result<None> SaveImage(Bitmap bitmap, string path)
        {
            if (bitmap == null)
                return Result.Fail<None>("Image was null");
            var result = Result.OfAction(() => bitmap.Save(path, ImageFormat));
            return result;
        }
    }
}
