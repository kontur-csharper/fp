using System.Drawing;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageSize
{
    public class ImageSizeConverter : IImageSizeConverter
    {
        public Result<Size> ConvertToSize(int[] sizeParameters)
        {
            if (sizeParameters.Length != 2 || sizeParameters[0] <= 0 || sizeParameters[1] <= 0)
                return new Result<Size>("Invalid number of size parameters or not positive parameters");
            return new Result<Size>(null, new Size(sizeParameters[0], sizeParameters[1]));
        }
    }
}