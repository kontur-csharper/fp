using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TagsCloud.Factory;
using TagsCloud.ImageProcessing.Config;
using TagsCloud.Layouter;
using TagsCloud.ResultOf;
using TagsCloud.TagsCloudProcessing.FontsConfig;
using TagsCloud.TextProcessing;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.TagsCloudProcessing.TagsGenerators
{
    public class TagsGenerator : ITagsGenerator
    {
        private readonly IServiceFactory<IRectanglesLayouter> layouterFactory;
        private readonly IWordConfig wordsConfig;
        private readonly IImageConfig imageConfig;

        public TagsGenerator(IServiceFactory<IRectanglesLayouter> layouterFactory, IWordConfig wordsConfig,
            IImageConfig imageConfig)
        {
            this.layouterFactory = layouterFactory;
            this.wordsConfig = wordsConfig;
            this.imageConfig = imageConfig;
        }

        public Result<IEnumerable<Tag>> CreateTags(IEnumerable<WordInfo> words)
        {
            var layouterResult = layouterFactory.Create();
            return layouterResult
                .Then(layouter => GetTagsFormWords(layouter, words));
        }

        private Result<IEnumerable<Tag>> GetTagsFormWords(IRectanglesLayouter layouter, IEnumerable<WordInfo> words)
        {
            var count = 30;
            return Result.Ok(words.OrderByDescending(info => info.Frequence)
                        .Take(count)
                        .Select((wordInfo, index) =>
                        {
                            using var font = new Font(wordsConfig.Font.FontFamily, count - index / 2);
                            var size = TextRenderer.MeasureText(wordInfo.Word, font);
                            return new Tag(wordInfo.Word,
                                layouter.PutNextRectangle(size),
                                new FontConfig(font.FontFamily, font.Size));
                        }))
                .Then(tags => CheckTagsFitOnImage(layouter, tags));
        }

        private Result<IEnumerable<Tag>> CheckTagsFitOnImage(IRectanglesLayouter layouter, IEnumerable<Tag> tags)
        {
            tags = tags.ToList();
            var layoutSize = layouter.GetLayoutSize();
            var subtraction = imageConfig.ImageSize - layoutSize;
            if (subtraction.Width < 0 || subtraction.Height < 0)
                return Result.Fail<IEnumerable<Tag>>(
                    $"Cloud out of size image:\n Cloud size: {layoutSize}\n Image size: {imageConfig.ImageSize}");
            return Result.Ok(tags);
        }
    }
}
