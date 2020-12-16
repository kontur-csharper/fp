using System.Collections.Generic;
using System.Drawing;
using TagsCloud.TextProcessing.Tags;

namespace TagsCloud.TagsCloudVisualization
{
    public interface IVisualization
    {
        Image GetImageCloud(IReadOnlyList<WordTag> tags, int cloudRadius);
    }
}