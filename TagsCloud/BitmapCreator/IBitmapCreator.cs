using System;
using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagsCloud.BitmapCreator
{
    public interface IBitmapCreator : IDisposable
    {
        public Result<Bitmap> Create(IReadOnlyCollection<string> words);
    }
}
