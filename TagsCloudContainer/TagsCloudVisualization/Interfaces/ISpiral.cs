using System.Drawing;

namespace TagsCloudContainer.TagsCloudVisualization.Interfaces
{
    public interface ISpiral
    {
        Point Center { get; }
        SpiralType Type { get; }
        Point GetNextPoint();
    }
}