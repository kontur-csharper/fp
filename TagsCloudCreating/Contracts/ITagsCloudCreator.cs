using System.Collections.Generic;
using TagsCloudCreating.Core.WordProcessors;
using TagsCloudCreating.Infrastructure;

namespace TagsCloudCreating.Contracts
{
    public interface ITagsCloudCreator
    {
        public Result<IEnumerable<Tag>> CreateTagsCloud(IEnumerable<string> words);
    }
}