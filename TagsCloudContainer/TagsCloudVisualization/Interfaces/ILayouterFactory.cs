namespace TagsCloudContainer.TagsCloudVisualization.Interfaces
{
    public interface ILayouterFactory
    {
        ILayouter GetLayouter(SpiralType type);
    }
}