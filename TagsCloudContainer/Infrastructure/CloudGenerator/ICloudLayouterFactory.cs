using ResultOf;

namespace TagsCloudContainer.Infrastructure.CloudGenerator
{
    internal interface ICloudLayouterFactory
    {
        public Result<ICloudLayouter> CreateCloudLayouter();
    }
}