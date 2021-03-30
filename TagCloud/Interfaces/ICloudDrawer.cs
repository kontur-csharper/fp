using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Interfaces
{
    public interface ICloudDrawer
    {
        Result<Bitmap> DrawCloud(Result<IEnumerable<WordForCloud>> wordsForCloud);
    }
}