using System.Collections.Generic;
using System.Drawing;

namespace WordCloudGenerator
{
    public interface IPainter
    {
        public delegate IPainter Factory(IPalette palette, FontFamily fontFamily);

        public Bitmap Paint(IEnumerable<GraphicString> words, Size imgSize);
    }
}