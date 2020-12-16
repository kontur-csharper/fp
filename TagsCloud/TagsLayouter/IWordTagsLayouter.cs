using System.Collections.Generic;
using ResultPattern;
using TagsCloud.TextProcessing.Tags;

namespace TagsCloud.TagsLayouter
{
    public interface IWordTagsLayouter
    {
        Result<(IReadOnlyList<WordTag>, int)> GetWordTagsAndCloudRadius(string text);
    }
}