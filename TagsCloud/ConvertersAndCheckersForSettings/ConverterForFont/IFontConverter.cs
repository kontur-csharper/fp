using System.Drawing;
using ResultPattern;

namespace TagsCloud.ConvertersAndCheckersForSettings.ConverterForFont
{
    public interface IFontConverter
    {
        Result<Font> ConvertToFont(string[] fontParameters);
    }
}