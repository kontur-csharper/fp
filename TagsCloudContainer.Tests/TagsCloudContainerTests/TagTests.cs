using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.TagsCloudContainer;

namespace TagsCloudVisualization.Tests.TagsCloudContainerTests
{
    public class TagTests
    {
        private string Text { get; set; }
        private Rectangle Rectangle { get; set; }
        private Font Font { get; set; }
        private Brush Brush { get; set; }

        [SetUp]
        public void SetUp()
        {
            Text = "text";
            Rectangle = new Rectangle(0, 0, 1, 1);
            Font = new Font("Arial", 2);
            Brush = Brushes.Black;
        }

        [Test]
        public void ShouldNotThrowException_WhenCorrectTagParameters()
        {
            Func<Tag> createTag = () => new Tag(Text, Rectangle, Font, Brush);

            createTag.Should().NotThrow();
        }

        [TestCase(-1, 0, 1, 1, TestName = "X is negative")]
        [TestCase(0, -1, 1, 1, TestName = "Y is negative")]
        [TestCase(0, 0, -1, 1, TestName = "Width is negative")]
        [TestCase(0, 0, 1, -1, TestName = "Height is negative")]
        public void ShouldThrowException_When(int x, int y, int width, int height)
        {
            Func<Tag> createTag = () => new Tag(Text, new Rectangle(x, y, width, height), Font, Brush);

            createTag.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ShouldThrowException_WhenFontIsNull()
        {
            Func<Tag> createTag = () => new Tag(Text, Rectangle, null, Brush);

            createTag.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ShouldThrowException_WhenBrushIsNull()
        {
            Func<Tag> createTag = () => new Tag(Text, Rectangle, Font, null);

            createTag.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ShouldThrowException_WhenFontFamilyDontExist()
        {
            var tag = new Tag(Text, Rectangle, Font, Brush);
            Func<Tag> changeFontFamily = () => tag.ChangeFontFamily("wrong");

            changeFontFamily.Should().Throw<ArgumentException>();
        }
    }
}