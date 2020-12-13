using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public class ImageSaver : IImageSaver
    {
        private readonly string imagePath;

        private static readonly Dictionary<string, ImageFormat> formats = new Dictionary<string, ImageFormat>
        {
            {".jpeg", ImageFormat.Jpeg},
            {".jpg", ImageFormat.Jpeg},
            {".tiff", ImageFormat.Tiff},
            {".tif", ImageFormat.Tiff},
            {".png", ImageFormat.Png},
            {".gif", ImageFormat.Gif},
            {".bmp", ImageFormat.Bmp}
        };

        public static string[] SupportedFormats => formats.Keys.ToArray();

        public ImageSaver(string imagePath)
        {
            this.imagePath = imagePath;
        }

        public void Save(Bitmap image)
        {
            var format = formats[Path.GetExtension(imagePath)];
            image.Save(imagePath, format);
        }
    }
}