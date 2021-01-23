using System.Collections.Generic;
using System.Linq;
using TagsCloudCreating.Contracts;
using TagsCloudCreating.Core.WordProcessors;
using TagsCloudCreating.Infrastructure;

namespace TagsCloudCreating.Core
{
    public class TagsCloudCreator : ITagsCloudCreator
    {
        private ITagsCloudLayouter Layouter { get; }
        private IWordHandler WordHandler { get; }
        private WordConverter WordConverter { get; }

        public TagsCloudCreator(ITagsCloudLayouter layouter, IWordHandler wordHandler, WordConverter wordConverter)
        {
            Layouter = layouter;
            WordHandler = wordHandler;
            WordConverter = wordConverter;
        }

        public Result<IEnumerable<Tag>> CreateTagsCloud(IEnumerable<string> words)
        {
            var readyTags = words.AsResult()
                .Then(WordHandler.NormalizeAndExcludeBoringWords)
                .Then(WordConverter.ConvertToTags)
                .ReplaceError(err => "MyStem.exe not found! " +
                                     "Please, go to: http://download.cdn.yandex.net/mystem/mystem-3.0-win7-32bit.zip"
                )
                .Then(InsertTagsInFrames)
                .Then(tags => tags.OrderByDescending(t => t.Frequency).AsEnumerable());
            Layouter.Reset();
            return readyTags;
        }

        private IEnumerable<Tag> InsertTagsInFrames(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                var frame = Layouter.PutNextRectangle(tag.GetCeilingSize());
                yield return tag.InsertTagInFrame(frame);
            }
        }
    }
}