using System.Drawing;
using ResultPattern;

namespace TagsCloud.TextProcessing.WordsMeasurer
{
    public interface IWordMeasurer
    {
        Result<Size> GetWordSize(string word, Font font);
    }
}