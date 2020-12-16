using ResultPattern;

namespace TagsCloud.ProcessorOfApp
{
    public interface IAppProcessor
    {
        Result<None> Run();
    }
}