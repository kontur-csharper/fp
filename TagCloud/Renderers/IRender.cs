using ResultOf;

namespace TagCloud.Renderers
{
    public interface IRender
    {
        Result<None> Render();
    }
}