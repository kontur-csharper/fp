using System.Drawing;
using ResultOf;

namespace TagCloud.Visualizers
{
    public interface IVisualizer<out T>
    {
        T VisualizeTarget { get; }
        Result<None> Draw(Graphics graphics);
    }
}
