using System.Drawing;

namespace TagCloud.Infrastructure.Graphics
{
    public interface IImageGenerator
    {
        public Result<Image> Generate();
    }
}