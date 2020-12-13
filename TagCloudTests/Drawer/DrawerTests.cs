using System;
using System.Drawing;
using NUnit.Framework;
using FakeItEasy;
using FluentAssertions;
using TagCloud;
using TagCloud.Drawers;
using TagCloud.Layouters;
using TagCloud.Settings;

namespace TagCloudTests.Drawer
{
    public class DrawerTests
    {
        private DrawerSettings settings;
        private IRectangleLayouter fakeLayouter;
        private Size fakeSize = new Size(50, 50);
        private TagInfo[] tags;
        
        [SetUp]
        public void SetUp()
        {
            settings = new DrawerSettings();
            fakeLayouter = A.Fake<IRectangleLayouter>();
            A.CallTo(fakeLayouter)
                .Where(call => call.Method.Name == "PutNextRectangle")
                .WithReturnType<Result<Rectangle>>()
                .Returns(new Rectangle(0, 0, 50, 50));
            tags = new[] {new TagInfo("string", 0.5), new TagInfo("otherString", 0.2)};
        }
        
        [Test]
        public void DrawResult_ShouldBeOk_WhenAllSettingsCorrect()
        {
            var drawer = new TagDrawer(settings, fakeLayouter);
            drawer.DrawTagCloud(tags).IsSuccess.Should().BeTrue();
        }
        
        [Test]
        public void DrawResult_ShouldBeFail_WhenNonExistentFont()
        {
            settings.FontFamily = "Some very strange font family";
            var drawer = new TagDrawer(settings, fakeLayouter);
            drawer.DrawTagCloud(tags).Error.Should().Be("Font wasn't found on the system.");
        }
        
        [Test]
        public void DrawResult_ShouldBeFail_WhenTagCloudOutOfBorder()
        {
            settings.ImageSize = new Size(25,200);
            var drawer = new TagDrawer(settings, fakeLayouter);
            drawer.DrawTagCloud(tags).Error.Should().Be("Tag cloud didn't fit on the image.\n" +
                                                        "Try increasing the image size or decreasing the font size");
        }
    }
}