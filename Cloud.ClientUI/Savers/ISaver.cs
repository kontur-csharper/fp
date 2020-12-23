using System.Drawing;
using TagsCloudVisualization;

namespace Cloud.ClientUI
{
    public interface ISaver
    {
        public Result<None> SaveImage(Bitmap bitmap, string fileName);
    }
}