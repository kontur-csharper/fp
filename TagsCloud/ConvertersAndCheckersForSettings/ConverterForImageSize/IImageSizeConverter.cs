using System.Drawing;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageSize
{
    public interface IImageSizeConverter
    {
        Result<Size> ConvertToSize(int[] sizeParameters);
    }
}