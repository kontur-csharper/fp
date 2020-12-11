using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagCloud.Core.ImageSavers
{
    public class AllExtensionsSaver : IImageSaver
    {
        private readonly Dictionary<string, ImageFormat> formats;

        public AllExtensionsSaver()
        {
            formats = new Dictionary<string, ImageFormat>(
                StringComparer.CurrentCultureIgnoreCase)
            {
                ["jpeg"] = ImageFormat.Jpeg,
                ["jpg"] = ImageFormat.Jpeg,
                ["png"] = ImageFormat.Png,
                ["gif"] = ImageFormat.Gif,
                ["bmp"] = ImageFormat.Bmp,
                ["tif"] = ImageFormat.Tiff
            };
        }

        public Result<string> Save(Bitmap bitmap, string fullPath, string format)
        {
            return formats.ContainsKey(format)
                ? Result
                    .Of(() =>
                    {
                        var path = $"{fullPath}.{format}";
                        bitmap.Save(path, formats[format]);
                        return path;
                    })
                    .RefineError("An error occured while saving file")
                : Result.Fail<string>(
                    $"Unsupported format {format}\nPlease use: {string.Join(", ", formats.Keys)}");
        }
    }
}