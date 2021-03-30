using System.Drawing;

namespace TagCloud.Interfaces
{
    public interface ITagCloudCreator
    {
        Result<Bitmap> GetCloud();
    }
}