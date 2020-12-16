using System.Collections.Generic;
using System.Drawing.Imaging;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageFormat
{
    public class ImageFormatConverter : IImageFormatConverter
    {
        private static Dictionary<string, ImageFormat> _imageFormats = new Dictionary<string, ImageFormat>
        {
            ["bmp"] = ImageFormat.Bmp,
            ["jpeg"] = ImageFormat.Jpeg,
            ["png"] = ImageFormat.Png,
            ["gif"] = ImageFormat.Gif,
            ["tiff"] = ImageFormat.Tiff
        };

        public Result<ImageFormat> ConvertToImageFormat(string imageFormatFromString)
        {
            return _imageFormats.TryGetValue(imageFormatFromString.ToLower(), out var format)
                ? new Result<ImageFormat>(null, format)
                : new Result<ImageFormat>("Doesn't contain this image format");
        }
    }
}