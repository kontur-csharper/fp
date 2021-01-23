using System.Collections.Generic;
using TagsCloudCreating.Infrastructure;
using TagsCloudVisualization.Infrastructure;

namespace TagsCloudVisualization.Contracts
{
    public interface IWordsReader
    {
        public Result<IEnumerable<string>> GetAllData(string source);
    }
}