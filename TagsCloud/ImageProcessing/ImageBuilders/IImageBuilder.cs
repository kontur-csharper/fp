using System.Collections.Generic;
using System.Drawing;
using TagsCloud.TagsCloudProcessing;

namespace TagsCloud.ImageProcessing.ImageBuilders
{
    public interface IImageBuilder
    {
        void DrawTags(IEnumerable<Tag> tags, Bitmap bitmap);
    }
}
