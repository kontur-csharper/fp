using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface ICloudDrawer
    {
        IColorProvider ColorProvider { get; set; }
        IImageSaver ImageSaver { get; set; }
        Result<None> DrawCloud(List<WordWithFont> words, string targetPath, string imageName);
        void ChangeImageSize(int newSize);
    }
}