using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudVisualization;

namespace TagsCloudVisualization.Tests.TagsCloudVisualizationTests
{
    public class BitmapSaverTests
    {
        private Bitmap ImageBitmap { get; set; }
        private BitmapSaver BitmapSaver { get; set; }

        [SetUp]
        public void SetUp()
        {
            const int height = 10;
            const int width = 10;
            using var bitmap = new Bitmap(width, height);
            ImageBitmap = bitmap;
            BitmapSaver = new BitmapSaver();
        }

        [Test]
        public void SaveBitmapToDirectory_ThrowException_WhenBitmapIsNull()
        {
            var path = $"..{Path.DirectorySeparatorChar}image.png";
            Action saveImage = () => BitmapSaver.SaveBitmapToDirectory(null, path);

            saveImage.Should().Throw<ArgumentException>();
        }

        [Test]
        public void SaveBitmapToDirectory_DoesntThrowException_WhenRightPath()
        {
            var path = $"..{Path.DirectorySeparatorChar}image.png";
            Action saveImage = () => BitmapSaver.SaveBitmapToDirectory(ImageBitmap, path);

            saveImage.Should().NotThrow();
        }

        [TestCaseSource(nameof(PathTestCases))]
        public void SaveBitmapToDirectory_ThrowException_When(string path)
        {
            Action saveImage = () => BitmapSaver.SaveBitmapToDirectory(ImageBitmap, path);

            saveImage.Should().Throw<ArgumentException>();
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