using System.Drawing;
using TagsCloud.ResultOf;

namespace TagsCloud.ImageProcessing.SaverImage.ImageSavers
{
    public interface IImageSaver
    {
        Result<None> SaveImage(Bitmap bitmap, string path);
        bool CanSave(string path);
    }
}
