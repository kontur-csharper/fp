using System.Drawing;
using System.Collections.Generic;
using TagsCloud.Layouter;

namespace TagsCloud.Drawer
{
    public class LayoutDrawer : ILayoutDrawer
    {
        private IEnumerable<Tag> layoutTags;

        public LayoutDrawer()
        {
            layoutTags = new List<Tag>();
        }

        public void AddTags(IEnumerable<Tag> tags)
        {
            layoutTags = tags;
        }

        public void Draw(Graphics graphics)
        {
            foreach (var tag in layoutTags)
            {
                var brush = new SolidBrush(tag.FontColor);
                using var font = new Font(tag.FontFamily, tag.FontSize);
                graphics.DrawString(tag.Text, font, brush, tag.Rectangle.Location);
            }
        }
    }
}