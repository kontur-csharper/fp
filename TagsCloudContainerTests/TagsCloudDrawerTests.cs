using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;
using TagsCloudVisualization;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class TagsCloudDrawerTests
    {
        private static IDrawSettings settings;
        private static ICloudLayouter layouter;
        private static IFontDetailsCreator fontDetailsCreator;
        private static IFontColorCreator fontColorCreator;
        private static TagsCloudDrawer drawer;

        [SetUp]
        public void SetUp()
        {
            settings = new AppSettings
            {
                ImageWidth = 300,
                ImageHeight = 300,
                BackgroundColorName = "Blue"
            };

            layouter = new CircularCloudLayouter(
                new Point(150, 150),
                new Spiral());

            fontDetailsCreator = new FontDetailsCreator("arial");
            fontColorCreator = new FontColorCreator(Color.Black);
            drawer = new TagsCloudDrawer(settings, layouter, fontDetailsCreator, fontColorCreator);
        }

        [TearDown]
        public void TearDown()
        {
            drawer.Dispose();
        }

        [Test]
        public void Draw_CreateImageWithWidthFromSettings()
        {
            var testBitmap = drawer.Draw(new Dictionary<string, int> {{"a", 2}});

            testBitmap.Width.Should().Be(settings.ImageWidth);
        }

        [Test]
        public void Draw_CreateImageWithHeightFromSettings()
        {
            var testBitmap = drawer.Draw(new Dictionary<string, int> {{"a", 2}});

            testBitmap.Height.Should().Be(settings.ImageHeight);
        }

        [Test]
        public void Draw_CanDrawSeveralImageBeforeDisposed()
        {
            var countedWords = new Dictionary<string, int>
            {
                {"one", 5},
                {"four", 3},
            };
            var first = drawer.Draw(countedWords);
            var second = drawer.Draw(countedWords);

            BitmapEquals(first, second).Should().BeTrue();
        }

        [Test]
        public void Draw_WithSameSettingReturnsSameResult()
        {
            var countedWords = new Dictionary<string, int>
            {
                {"one", 5},
                {"two", 1},
                {"three", 1},
                {"four", 3},
                {"five", 1}
            };
            var expectedBitmap = new Bitmap("../../../TestFiles/expectedImage.png");
            
            var actualBitmap = drawer.Draw(countedWords);

            BitmapEquals(actualBitmap, expectedBitmap).Should().BeTrue();
        }

        private static bool BitmapEquals(Bitmap first, Bitmap second)
        {
            if (first.Width != second.Width || first.Height != second.Height)
                return false;

            for (int x = 0; x < first.Width; x++)
            for (int y = 0; y < first.Height; y++)
            {
                if (first.GetPixel(x, y) != second.GetPixel(x, y))
                    return false;
            }

            return true;
        }
    }
}