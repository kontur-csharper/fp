using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.App.CloudGenerator;

namespace TagsCloudContainer.Infrastructure.CloudVisualizer
{
    internal interface ICloudPainter
    {
        public Result<None> Paint(IEnumerable<Tag> cloud, Graphics g);
    }
}