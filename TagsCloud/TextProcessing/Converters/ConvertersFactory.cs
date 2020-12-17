using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.TextProcessing.Converters
{
    public class ConvertersFactory : ServiceFactory<IWordConverter>
    {
        private readonly IWordConfig wordsConfig;

        public ConvertersFactory(IWordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public override Result<IWordConverter> Create()
        {
            var converterNames = wordsConfig.ConvertersNames;
            var convertersResult = converterNames
                                     .Select(name => Result.Of(() => services[name](), $"This converter {name} not supported"))
                                     .ToArray();

            if (convertersResult.Any(result => !result.IsSuccess))
                return convertersResult.Aggregate((working, current) => current.RefineError(working.Error));

            var compositeConverter = new CompositeConverter(convertersResult
                                                                            .Select(res => res.Value)
                                                                            .ToArray());
            return compositeConverter;
        }

        private class CompositeConverter : IWordConverter
        {
            private readonly IWordConverter[] converters;

            public CompositeConverter(IWordConverter[] converters)
            {
                this.converters = converters;
            }

            public string Convert(string word) =>
                converters.Aggregate(word, (current, converter) => converter.Convert(current));
        }
    }
}
