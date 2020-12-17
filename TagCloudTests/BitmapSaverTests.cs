using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.BitmapSaver;

namespace TagCloudTests
{
    public class BitmapSaverTests
    {
        [Test]
        public void Save_ShouldThrow_WhenIncorrectFormat()
        {
            BitmapSaver.Save(new Bitmap(100, 100), "jpga").IsSuccess.Should().BeFalse();
        }

        [Test]
        public void Save_ShouldThrow_WhenIncorrectPath()
        {
            BitmapSaver.Save(new Bitmap(100, 100), "png", "blablapath").IsSuccess.Should().BeFalse();
        }
    }
}
