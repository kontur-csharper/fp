using System.Collections.Generic;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing;

namespace TagsCloud.TagsCloudProcessing.TagsGenerators
{
    public interface ITagsGenerator
    {
        Result<IEnumerable<Tag>> CreateTags(IEnumerable<WordInfo> words);
    }
}
