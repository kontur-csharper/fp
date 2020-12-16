using System.Collections.Generic;
using System.Drawing;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using RectanglesCloudLayouter.LayouterOfRectangles;
using ResultPattern;
using TagsCloud.TagsLayouter;
using TagsCloud.TextProcessing.FrequencyOfWords;
using TagsCloud.TextProcessing.Tags;
using TagsCloud.TextProcessing.WordsMeasurer;

namespace TagsCloudTests.UnitTests.WordTagsLayouter_Tests
{
    public class WordTagsLayouter_Should
    {
        private WordTagsLayouter _sut;
        private IWordsFrequency _wordsFrequency;
        private IRectanglesLayouter _rectanglesLayouter;
        private IWordMeasurer _wordMeasurer;
        private static readonly Font _font = new Font("arial", 5);

        [SetUp]
        public void SetUp()
        {
            _wordsFrequency = A.Fake<IWordsFrequency>();
            _rectanglesLayouter = A.Fake<IRectanglesLayouter>();
            _wordMeasurer = A.Fake<IWordMeasurer>();
            _sut = new WordTagsLayouter(_wordsFrequency, _rectanglesLayouter, _wordMeasurer, _font);
        }

        [Test]
        public void GetWordTagsAndCloudRadius_TagsAndRadiusFailResult_WhenStringIsNull()
        {
            var act = _sut.GetWordTagsAndCloudRadius(null);

            act.Should()
                .BeEquivalentTo(
                    ResultExtensions.Fail<(IReadOnlyList<WordTag>, int)>("String for tags layouter must be not null"));
        }

        [Test]
        public void GetWordTagsAndCloudRadius_BeNotCalledAnyMethodDependencies_WhenStringIsNull()
        {
            _sut.GetWordTagsAndCloudRadius(null);

            A.CallTo(() => _wordsFrequency.GetWordsFrequency(A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _wordMeasurer.GetWordSize(A<string>.Ignored, A<Font>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => _rectanglesLayouter.PutNextRectangle(A<Size>.Ignored)).MustNotHaveHappened();
        }

        [TestCase("")]
        [TestCase("игра")]
        public void GetWordTagsAndCloudRadius_BeCalledGetWordsFrequencyOnce_WhenStringIsNotNull(string text)
        {
            A.CallTo(() => _wordsFrequency.GetWordsFrequency(A<string>.Ignored))
                .Returns(ResultExtensions.Ok(new Dictionary<string, int>()));
            A.CallTo(() => _wordMeasurer.GetWordSize(A<string>.Ignored, A<Font>.Ignored))
                .Returns(ResultExtensions.Ok(new Size()));

            _sut.GetWordTagsAndCloudRadius(text);

            A.CallTo(() => _wordsFrequency.GetWordsFrequency(A<string>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetWordTagsAndCloudRadius_BeCalledGetWordSizeCertainNumber_WhenStringIsNotNull()
        {
            var text = "игра теннис";
            var words = text.Split(' ');
            A.CallTo(() => _wordsFrequency.GetWordsFrequency(text)).Returns(ResultExtensions.Ok(
                new Dictionary<string, int>
                    {[words[0]] = 1, [words[1]] = 1}));

            _sut.GetWordTagsAndCloudRadius(text);

            A.CallTo(() => _wordMeasurer.GetWordSize(A<string>.Ignored, A<Font>.Ignored))
                .MustHaveHappened(text.Split(' ').Length, Times.Exactly);
        }

        [Test]
        public void GetWordTagsAndCloudRadius_BeCalledPutNextRectangleCertainNumber_WhenStringIsNotNull()
        {
            var text = "игра теннис";
            var words = text.Split(' ');
            A.CallTo(() => _wordsFrequency.GetWordsFrequency(text)).Returns(ResultExtensions.Ok(
                new Dictionary<string, int>
                    {[words[0]] = 1, [words[1]] = 1}));

            _sut.GetWordTagsAndCloudRadius(text);

            A.CallTo(() => _rectanglesLayouter.PutNextRectangle(A<Size>.Ignored))
                .MustHaveHappened(text.Split(' ').Length, Times.Exactly);
        }

        [Test]
        public void GetWordTagsAndCloudRadius_CertainNumberTags_WhenStringIsNotNull()
        {
            var text = "игра теннис";
            var words = text.Split(' ');
            A.CallTo(() => _wordsFrequency.GetWordsFrequency(text)).Returns(ResultExtensions.Ok(
                new Dictionary<string, int>
                    {[words[0]] = 1, [words[1]] = 1}));

            var act = _sut.GetWordTagsAndCloudRadius(text);

            act.GetValueOrThrow().Item1.Should().HaveCount(words.Length);
        }
    }
}