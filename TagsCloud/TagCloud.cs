using TagsCloud.Drawer;
using TagsCloud.ResultOf;
using TagsCloud.WordsParser;

namespace TagsCloud
{
    public class TagCloud : ITagCloud
    {
        private readonly IRectangleLayout rectangleLayout;
        private readonly IWordsAnalyzer wordsAnalyzer;

        public TagCloud(IWordsAnalyzer wordsAnalyzer, IRectangleLayout rectangleLayout)
        {
            this.wordsAnalyzer = wordsAnalyzer;
            this.rectangleLayout = rectangleLayout;
        }

        public Result<None> MakeTagCloud() => 
            wordsAnalyzer.AnalyzeWords()
                .Then(rectangleLayout.PlaceWords)
                .Then(_ => rectangleLayout.DrawLayout());

        public Result<None> SaveTagCloud() => Result.OfAction(() => rectangleLayout.SaveLayout());
    }
}