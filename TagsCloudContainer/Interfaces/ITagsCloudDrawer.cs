using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudContainer
{
    public interface ITagsCloudDrawer : IDisposable
    {
        Bitmap Draw(Dictionary<string, int> countedWords);
    }
}