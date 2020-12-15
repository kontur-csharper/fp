using System.Collections.Generic;
using ResultOf;
using TagsCloudContainer.App.CloudGenerator;

namespace TagsCloudContainer.Infrastructure.CloudGenerator
{
    internal interface ICloudGenerator
    {
        public Result<IEnumerable<Tag>> GenerateCloud(Dictionary<string, double> frequencyDictionary);
    }
}