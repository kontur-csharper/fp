using System.Collections.Generic;
using TagCloud.ExceptionHandler;

namespace TagCloud
{
    public interface ICloudLayouter
    {
        public Result<List<RectangleWithWord>> GetRectangles(Result<SizeWithWord[]> sizesResult);
    }
}