using System.Collections.Generic;
using System.IO;
using ResultOf;
using TagsCloudContainer.Infrastructure.DataReader;

namespace TagsCloudContainer.App.DataReader
{
    public class TxtFileReader : IDataReader
    {
        private readonly string filename;
        public TxtFileReader(string filename)
        {
            this.filename = filename;
        }
        public Result<IEnumerable<string>> ReadLines()
        {
            if (!File.Exists(filename))
                return Result.Fail<IEnumerable<string>>("Input file is not found");
            return Result.Of(() => File.ReadLines(filename)).RefineError("Can't read input file");
        }
    }
}