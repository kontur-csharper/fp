using System.Drawing;

namespace TagsCloudContainer
{
    public interface IColorProvider
    {
        Result<Color> GetNextColor();
    }
}