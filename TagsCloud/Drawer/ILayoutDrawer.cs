using System.Collections.Generic;
using System.Drawing;
using TagsCloud.Layouter;

namespace TagsCloud.Drawer
{
    public interface ILayoutDrawer
    {
        public void AddTags(IEnumerable<Tag> tags);

        public void Draw(Graphics graphics);
    }
}