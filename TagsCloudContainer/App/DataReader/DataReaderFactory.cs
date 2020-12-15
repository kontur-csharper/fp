using System.IO;
using ResultOf;
using TagsCloudContainer.App.Settings;
using TagsCloudContainer.Infrastructure.DataReader;

namespace TagsCloudContainer.App.DataReader
{
    public class DataReaderFactory : IDataReaderFactory
    {
        private readonly InputSettings settings;

        public DataReaderFactory(InputSettings settings)
        {
            this.settings = settings;
        }

        public Result<IDataReader> CreateDataReader()
        {
            switch (Path.GetExtension(settings.InputFileName))
            {
                case ".txt":
                    return new TxtFileReader(settings.InputFileName);
                case ".docx":
                    return new WordFileReader(settings.InputFileName);
            }

            return Result.Fail<IDataReader>("Unknown input file format");
        }
    }
}