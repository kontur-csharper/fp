using ResultOf;

namespace TagsCloudContainer.Infrastructure.CloudVisualizer
{
    public interface ICloudVisualizer
    {
        public Result<None> Visualize();
    }
}