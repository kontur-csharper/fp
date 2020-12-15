using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.Interfaces
{
    public interface IBitmapSaver
    {
        void SaveBitmapToDirectory(Bitmap imageBitmap, string savePath);
    }
}