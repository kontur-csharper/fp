using System;
using System.Drawing;

namespace TagsCloudVisualization.CloudTags
{
    public interface ICloudTag : IEquatable<ICloudTag>
    {
        public Rectangle Rectangle { get; }
        public string Text { get; }
    }
}