using System.Drawing;

namespace TagsCloudContainer.Common
{
    public interface IImageHolder
    {
        Result<Size> GetImageSize();
        Result<Graphics> StartDrawing();
        void UpdateUi();
        void RecreateImage(ImageSettings settings);
        Result<None> SaveImage(string fileName);
    }
}