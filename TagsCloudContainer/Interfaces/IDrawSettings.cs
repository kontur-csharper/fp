using System.Drawing;

namespace TagsCloudContainer
{
    public interface IDrawSettings
    {
        Color BackgroundColor { get; }
        int ImageWidth { get; }
        int ImageHeight { get; }
    }
}