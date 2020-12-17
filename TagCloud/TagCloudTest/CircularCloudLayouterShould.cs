using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagCloud;
using TagCloud.Curves;
using TagCloud.Visualization;
using TagCloud.WordsFilter;
using TagCloud.WordsProvider;

namespace TagCloudTest
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;
            var fileName = $"{TestContext.CurrentContext.Test.Name}_Failed.jpg";
            var path = $"../../../FailedTests/{fileName}";
            var visualizer = new TagCloudTagCloudVisualizer(GetDefaultTagCloud());
            var image = visualizer.CreateTagCloudBitMap(1920, 1080,
                new[] {Color.Blue, Color.Aqua},
                "Times New Roman");
            image.GetValueOrThrow().Save(path);
        }

        [Test]
        public void PutNextRectangle_DoesntThrow()
        {
            Assert.DoesNotThrow(() => GetDefaultTagCloud().PutNextWord("abc", new Size(10, 10)));
        }

        [Test]
        public void PutNextRectangle_PutsAllRectangles()
        {
            var expectedRectanglesCount = 15;
            var tagCloud = GetDefaultTagCloud();
            for (var i = 0; i < expectedRectanglesCount; i++)
                tagCloud.PutNextWord("abc", GetRandomSize());

            tagCloud.WordRectangles.Should().HaveCount(expectedRectanglesCount);
        }

        [Test]
        public void PutNextRectangle_PutsFirstRectangleInCenter()
        {
            var center = new Point(10, 18);
            var shiftedTagCloud = new CircularCloudLayouter(new ArchimedeanSpiral(center),
                GetDefaultWordsProvider(), GetDefaultWordsFilter());
            shiftedTagCloud.PutNextWord("dsadsa", new Size(10, 5));

            shiftedTagCloud.WordRectangles[0].Rectangle.Location.Should().Be(center);
        }

        [Test]
        public void Rectangles_ShouldNotIntersect()
        {
            var tagCloud = GetDefaultTagCloud();
            for (var i = 0; i < 100; i++)
                tagCloud.PutNextWord("dadas", GetRandomSize());

            foreach (var wordRectangle in tagCloud.WordRectangles)
                tagCloud.WordRectangles.All(
                        other => other.Equals(wordRectangle)
                                 || !other.Rectangle.IntersectsWith(wordRectangle.Rectangle))
                    .Should().BeTrue();
        }

        [Test]
        public void ContainUniqueWords()
        {
            var tagCloud = GetDefaultTagCloud();
            tagCloud.GenerateTagCloud();

            tagCloud.WordRectangles.Select(wordRectangle => wordRectangle.Word).Should()
                .BeEquivalentTo(GetDefaultWordsFilter()
                    .Apply(GetDefaultWordsProvider().GetWords().GetValueOrThrow().ToHashSet()));
        }

        [Test]
        [Timeout(200)]
        public void Put1000Rectangles_InSufficientTime()
        {
            for (var i = 0; i < 1000; i++)
                GetDefaultTagCloud().PutNextWord("asda", GetRandomSize());
        }

        private readonly Random rnd = new Random();

        private Size GetRandomSize()
        {
            return new Size(rnd.Next() % 100 + 1, rnd.Next() % 100 + 1);
        }

        private IWordsProvider GetDefaultWordsProvider()
        {
            return new CircularWordsProvider();
        }

        private IWordsFilter GetDefaultWordsFilter()
        {
            return new WordsFilter().Normalize();
        }

        private ITagCloud GetDefaultTagCloud()
        {
            var spiral = new ArchimedeanSpiral(new Point(0, 0));
            var wordsProvider = GetDefaultWordsProvider();
            var wordsFilter = GetDefaultWordsFilter();
            return new CircularCloudLayouter(spiral, wordsProvider, wordsFilter);
        }
    }
}