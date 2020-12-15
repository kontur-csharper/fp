using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.Infrastructure.DataReader
{
    public interface IDataReader
    {
        public Result<IEnumerable<string>> ReadLines();
    }
}