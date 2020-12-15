using ResultOf;

namespace TagsCloudContainer.Infrastructure.DataReader
{
    public interface IDataReaderFactory
    {
        public Result<IDataReader> CreateDataReader();
    }
}