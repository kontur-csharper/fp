using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.Interfaces
{
    public interface ILayouter
    {
        Rectangle PutNextRectangle(Size rectangleSize);
    }
}