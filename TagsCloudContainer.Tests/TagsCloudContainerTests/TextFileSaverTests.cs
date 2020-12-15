using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudContainer;
using TagsCloudContainer.TagsCloudContainer.Interfaces;

namespace TagsCloudVisualization.Tests.TagsCloudContainerTests
{
    public class TextFileSaverTests
    {
        [Test]
        public void DoesntThrowException_WhenPathIsOk()
        {
            var path = $"..{Path.DirectorySeparatorChar}image.png";

            Func<ITextSaver> getFileSaver = () => new TextFileSaver(path);

            getFileSaver.Should().NotThrow();
        }

        [TestCaseSource(nameof(PathTestCases))]
        public void ThrowException_When(string path)
        {
            Func<ITextSaver> getFileSaver = () => new TextFileSaver(path);

            getFileSaver.Should().Throw<ArgumentException>();
        }

        private static IEnumerable<TestCaseData> PathTestCases()
        {
            yield return new TestCaseData("<html></html>").SetName("Directory dont exist");
            yield return new TestCaseData(".. Dir text.txt").SetName("Not platform separator");
            yield return new TestCaseData($@"..{Path.DirectorySeparatorChar}text txt").SetName(
                "Doesnt have dot separator");
            yield return new TestCaseData($@"..{Path.DirectorySeparatorChar}text.").SetName(
                "Doesnt have filename extension");
        }
    }
}