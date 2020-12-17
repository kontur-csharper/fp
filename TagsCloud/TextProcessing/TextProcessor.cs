using System.Collections.Generic;
using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing.Converters;
using TagsCloud.TextProcessing.TextFilters;
using TagsCloud.TextProcessing.TextReaders;

namespace TagsCloud.TextProcessing
{
    public class TextProcessor
    {
        private readonly IServiceFactory<IWordsReader> readersFactory;
        private readonly IServiceFactory<ITextFilter> filtersFactory;
        private readonly IServiceFactory<IWordConverter> convertersFactory;

        public TextProcessor(IServiceFactory<IWordsReader> readers, IServiceFactory<ITextFilter> filters,
            IServiceFactory<IWordConverter> converters)
        {
            readersFactory = readers;
            filtersFactory = filters;
            convertersFactory = converters;
        }

        public Result<IEnumerable<WordInfo>> ReadFromFile(string path)
        {
            var wordsFromFile = readersFactory.Create().Then(reader => reader.ReadWords(path));

            var filteredWords = filtersFactory.Create()
                .Then(filter => wordsFromFile.Then(words => words.Where(word => filter.CanTake(word))));

            var convertedWords = convertersFactory.Create()
                .Then(converter => filteredWords.Then(words => words.Select(word => converter.Convert(word))));

            return convertedWords
                .Then(words => words.GroupBy(w => w).Select(g => new WordInfo(g.Key, g.Count())));
        }
    }
}
