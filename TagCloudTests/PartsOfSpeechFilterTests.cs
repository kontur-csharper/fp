﻿using FluentAssertions;
using NUnit.Framework;
using TagsCloud.WordFilters;

namespace TagCloudTests
{
    public class PartsOfSpeechFilterTests
    {
        [Test]
        public void FilterWords_ShouldNotContainsPartOfSpeech_WhenExcludeIt()
        {
            var filter = new PartsOfSpeechFilter(PartsOfSpeech.CONJ);
            var words = new[] {"и", "всем", "привет"};

            var result = filter.FilterWords(words);

            result.GetValueOrThrow().Should().NotContain("и");
        }
    }
}
