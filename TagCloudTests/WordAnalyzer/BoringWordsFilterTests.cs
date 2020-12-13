using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using TagCloud.WordsAnalyzer.WordFilters;

namespace TagCloudTests.WordAnalyzer
{
    [TestFixture]
    public class BoringWordsFilterTests
    {
        private BoringWordFilter filter;
        
        [SetUp]
        public void SetUp()
        {
            var boringWords = new HashSet<string> {"foo", "barbar", "foofoo"};
            filter = new BoringWordFilter(boringWords);
        }
        
        [Test]
        public void MethodShouldExclude_ShouldReturnFalse_WhenWordNotInBoringWords()
        {
            filter.ShouldExclude("bar").GetValueOrThrow().Should().BeFalse();
        }
        
        [Test]
        public void MethodShouldExclude_ShouldReturnTrue_WhenWordInBoringWords()
        {
            filter.ShouldExclude("foo").GetValueOrThrow().Should().BeTrue();
        }
    }
}