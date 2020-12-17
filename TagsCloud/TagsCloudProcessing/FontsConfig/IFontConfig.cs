using System.Drawing;

namespace TagsCloud.TagsCloudProcessing.FontsConfig
{
    public interface IFontConfig
    {
        FontFamily FontFamily { get; }
        float Size { get; }
    }
}
