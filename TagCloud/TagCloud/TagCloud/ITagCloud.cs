using System.Collections.Generic;
using System.Drawing;
using TagCloud.ErrorHandling;

namespace TagCloud
{
    public interface ITagCloud
    {
        List<WordRectangle> WordRectangles { get; }
        Result<ITagCloud> GenerateTagCloud();
        WordRectangle PutNextWord(string word, Size rectangleSize);
    }
}