using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ResultOf;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.TextProcessing.TextFilters
{
    public class FiltersFactory : ServiceFactory<ITextFilter>
    {
        private readonly IWordConfig wordsConfig;

        public FiltersFactory(IWordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public override Result<ITextFilter> Create()
        {
            var filterNames = wordsConfig.FilersNames;
            var filtersResult = filterNames.Select(name => Result.Of(() => services[name](), $"This filter {name} not supported")).ToArray();

            if (filtersResult.Any(res => !res.IsSuccess))
                return filtersResult.Aggregate((working, current) => current.RefineError(working.Error));

            return new CompositeFilter(filterNames.Select(name => services[name]()).ToArray());
        }

        private class CompositeFilter : ITextFilter
        {
            private readonly ITextFilter[] filters;

            public CompositeFilter(ITextFilter[] filters)
            {
                this.filters = filters;
            }

            public bool CanTake(string word) => filters.All(filter => filter.CanTake(word));
        }
    }
}
