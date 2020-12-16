using System.Drawing.Imaging;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageFormat
{
    public interface IImageFormatConverter
    {
        Result<ImageFormat> ConvertToImageFormat(string imageFormatFromString);
    }
}