using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.TextFileParser;
using TagCloud.Visualizer;

namespace TagCloud.Tests
{
    public class ExceptionHandlerTests
    {
        [Test]
        public void GetWords_ReturnResultWithError_WhenFileNotExist()
        {
            var fileParser = new FileParser(new[] {new TxtFileParser()});

            var result = fileParser.GetWords("not-existing-file.txt",
                Path.Combine(Directory.GetCurrentDirectory(), "test-files"));

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBe(null);
        }

        [Test]
        public void DrawBitmap_ReturnResultWithError_WhenRectsAreOutOfImage()
        {
            var imageOptions = new ImageOptions();
            var rectsWithWords = new List<RectangleWithWord>
            {
                new RectangleWithWord(new Rectangle(0,
                        0,
                        imageOptions.ImageWidth + 1,
                        imageOptions.ImageHeight + 1),
                    new Word(default, default))
            };

            var result = BitmapCreator.DrawBitmap(rectsWithWords, imageOptions);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Прямоугольники не поместились в масштаб изображения.");
        }

        [Test]
        public void DrawBitmap_ReturnResultWithError_WhenColorNotExists()
        {
            var imageOptions = new ImageOptions {ColorName = "not-existing-color"};
            var rectsWithWords = new List<RectangleWithWord>
            {
                new RectangleWithWord(new Rectangle(0,
                        0,
                        imageOptions.ImageWidth,
                        imageOptions.ImageHeight),
                    new Word(default, default))
            };

            var result = BitmapCreator.DrawBitmap(rectsWithWords, imageOptions);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBe(null);
        }

        [Test]
        public void DrawBitmap_ReturnResultWithError_WhenFontNotExists()
        {
            var imageOptions = new ImageOptions {FontName = "not-existing_font"};
            var rectsWithWords = new List<RectangleWithWord>
            {
                new RectangleWithWord(new Rectangle(0,
                        0,
                        imageOptions.ImageWidth,
                        imageOptions.ImageHeight),
                    new Word(default, default))
            };

            var result = BitmapCreator.DrawBitmap(rectsWithWords, imageOptions);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBe(null);
        }
    }
}