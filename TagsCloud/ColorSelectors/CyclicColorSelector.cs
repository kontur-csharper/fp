using System;
using System.Drawing;

namespace TagsCloud.ColorSelectors
{
    public class CyclicColorSelector : IColorSelector
    {
        private readonly Color[] colors;
        private int current;
        
        public CyclicColorSelector(Color[] colors)
        {
            if (colors == null || colors.Length == 0) throw new ArgumentException("Empty colors");
            this.colors = colors;
        }
        
        public Color Next()
        {
            return colors[current++ % colors.Length];
        }
    }
}