﻿using System.Drawing;

namespace TagsCloudVisualization.CloudTags
{
    public class CloudTag : ICloudTag
    {
        public CloudTag(Rectangle rectangle, string text)
        {
            Rectangle = rectangle;
            Text = text;
        }

        public bool Equals(ICloudTag tag)
        {
            return tag != null && Text == tag.Text && Rectangle.Equals(tag.Rectangle);
        }

        public Rectangle Rectangle { get; }
        public string Text { get; }
    }
}