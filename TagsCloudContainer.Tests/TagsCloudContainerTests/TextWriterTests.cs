using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudContainer;
using TagsCloudContainer.TagsCloudContainer.Interfaces;

namespace TagsCloudVisualization.Tests.TagsCloudContainerTests
{
    public class TextWriterTests
    {
        private ITextSaver saver;
        private TextWriter writer;

        [SetUp]
        public void SetUp()
        {
            writer = new TextWriter();
            saver = A.Fake<ITextSaver>();
        }

        [Test]
        public void ThroeException_WhenTextIsNull()
        {
            Assert.Throws<ArgumentException>(() => writer.WriteText(null, saver));
        }

        [Test]
        public void SaveFileOnce_WhenCalled()
        {
            writer.WriteText("text", saver);

            A.CallTo(() => saver.Save("text")).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SaveFileOnce_WhenManyCalls()
        {
            writer.WriteText("text1", saver);
            writer.WriteText("text2", saver);

            A.CallTo(() => saver.Save("text1")).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SaveFileMoreThanOnce_WhenManyCalls()
        {
            writer.WriteText("text1", saver);
            writer.WriteText("text1", saver);
            writer.WriteText("text1", saver);

            A.CallTo(() => saver.Save("text1")).MustHaveHappened(3, Times.Exactly);
        }

        [Test]
        public void SaveMustNotHaveHappened_WhenWrongText()
        {
            writer.WriteText("text", saver);

            A.CallTo(() => saver.Save("wow")).MustNotHaveHappened();
        }

        [TestCaseSource(nameof(TestCases))]
        public void ParseTextRight_When(string text, string expectedResult)
        {
            writer.WriteText(text, saver);

            A.CallTo(() => saver.Save(expectedResult)).MustHaveHappened();
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData("one two three", $"one{Environment.NewLine}two{Environment.NewLine}three")
                .SetName("Just words");
            yield return new TestCaseData("", "").SetName("Empty string");
            yield return new TestCaseData("Dot, net.", $"Dot{Environment.NewLine}net").SetName("With delimiters");
            yield return new TestCaseData("12 34", $"12{Environment.NewLine}34").SetName("Digits");
            yield return new TestCaseData($"New. {Environment.NewLine} line.", $"New{Environment.NewLine}line")
                .SetName("New line");
        }
    }
}