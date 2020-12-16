using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface IFontSizeCalculator
    {
        Result<IEnumerable<WordWithFont>> CalculateFontSize(IEnumerable<string> words, FontFamily fontFamily);
    }
}