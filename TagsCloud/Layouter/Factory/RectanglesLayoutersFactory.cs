using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.Layouter.Factory
{
    public class RectanglesLayoutersFactory : ServiceFactory<IRectanglesLayouter>
    {
        private readonly IWordConfig wordsConfig;

        public RectanglesLayoutersFactory(IWordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public override Result<IRectanglesLayouter> Create() =>
             Result.Of(() => services[wordsConfig.LayouterName](),
                $"This layouter {wordsConfig.LayouterName ?? "null"} not supported, choose layouter from available");
    }
}
