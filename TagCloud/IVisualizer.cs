using System.Drawing;
using ResultOf;

namespace TagCloud
{
    public interface IVisualizer
    {
        Result<string> Visualize(string filename, FontFamily fontFamily, Color stringColor);
    }
}