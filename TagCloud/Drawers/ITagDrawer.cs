using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud.Drawers
{
    public interface ITagDrawer
    {
        public Result<Bitmap> DrawTagCloud(IReadOnlyCollection<TagInfo> tags);
    }
}