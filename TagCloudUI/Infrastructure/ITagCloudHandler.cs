using System.Drawing;
using TagCloud.Core;

namespace TagCloudUI.Infrastructure
{
    public interface ITagCloudHandler
    {
        Result<Bitmap> Run(ITagCloudSettings settings);
    }
}