using System.Drawing;
using ResultPattern;

namespace TagsCloud.Saver
{
    public interface IImageSaver
    {
        Result<None> Save(Image image);
    }
}