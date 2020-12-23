using System.Collections.Generic;
using TagsCloudVisualization.CloudTags;

namespace TagsCloudVisualization.WordsConverters
{
    public interface IWordConverter
    {
        public Result<List<ICloudTag>> ConvertWords(List<string> words);
    }
}