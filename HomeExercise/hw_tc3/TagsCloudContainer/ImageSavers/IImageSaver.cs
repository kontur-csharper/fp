using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer
{
    public interface IImageSaver
    {
        string FormatName { get; set; }
        ImageFormat Format { get; set; }

        Result<None> Save(string path, string name, Bitmap bitmap);
    }
}