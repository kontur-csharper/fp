using Autofac;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloudContainer;
using FluentAssertions;

namespace TagsCloudContainer_Tests
{
    [TestFixture]
    class TagsCloudContainer_Tests
    {
        [TestCase("")]
        [TestCase("asasas")]
        [TestCase("Tahomaaa")]
        public void SetFontFamily_ReturnsFalse_OnInvalidFontFamily(string fontFamily)
        {
            var result = GetCreator().SetFontFamily(fontFamily);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Шрифт с таким именем не найден в системе");
        }

        [TestCase("times new roman")]
        [TestCase("Tahoma")]
        public void SetFontFamily_ReturnsTrue_OnValidFontFamily(string fontFamily)
        {
            GetCreator().SetFontFamily(fontFamily).IsSuccess.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase("asasas")]
        [TestCase("синий")]
        public void SetFontColor_ReturnsFalse_OnInvalidColorName(string color)
        {
            var result = GetCreator().SetFontColor(color);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Такой цвет не поддерживается");
        }

        [TestCase("cyan")]
        [TestCase("Blue")]
        public void SetFontColor_ReturnsTrue_OnValidColorName(string color)
        {
            GetCreator().SetFontColor(color).IsSuccess.Should().BeTrue();
        }

        [TestCase(-1)]
        [TestCase(99)]
        [TestCase(2001)]
        public void SetImageSize_ReturnsFalse_OnInvalidSize(int size)
        {
            var result = GetCreator().SetImageSize(size);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Неверный размер изображения \n(не больше 2000px и не меньше 100px)");
        }

        [TestCase(1000)]
        [TestCase(2000)]
        [TestCase(100)]
        public void SetImageSize_ReturnsTrue_OnValidSize(int size)
        {
            GetCreator().SetImageSize(size).IsSuccess.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase("txt")]
        [TestCase("image")]
        public void SetImageFormat_ReturnsFalse_OnInvalidFormat(string imageFormat)
        {
            var result = GetCreator().SetImageFormat(imageFormat);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Такой фрмат изображения не поддерживается");
        }

        [TestCase("png")]
        [TestCase("jpeg")]
        public void SetImageFormat_ReturnsTrue_OnValidFormat(string imageFormat)
        {
            var creator = GetCreator();
            creator.SetImageFormat(imageFormat).IsSuccess.Should().BeTrue();
            creator.GetImageFormat().Should().Be(imageFormat);
        }

        [Test]
        public void FixedColorProvider_ReturnsSameColor_OnManyCalls()
        {
            var color = Color.Red;
            var colorProvider = new FixedColorProvider(color);
            for (var i = 0; i < 5; i++)
                colorProvider.GetNextColor().Value.Should().Be(color);
        }

        [Test]
        public void StopWordsFilter_FilterAllStopWords_AndMakeLowerCase()
        {
            var filter = new StopWordsFilter(new StopWords());
            var input = new List<string> {"Abc", "of", "cba", "IN", "the", "car"};
            filter.Filter(input).Value.Should().BeEquivalentTo(new List<string> { "abc", "cba", "car" });
        }

        [Test]
        public void AddStopWord_WorksCorrectly()
        {
            var stopwords = new StopWords();
            var filter = new StopWordsFilter(stopwords);
            var input = new List<string> { "Abc", "of", "cba", "IN", "the", "car" };
            stopwords.Add("cAr");
            filter.Filter(input).Value.Should().BeEquivalentTo(new List<string> { "abc", "cba"});
        }

        [Test]
        public void DeleteStopWord_WorksCorrectly()
        {
            var stopwords = new StopWords();
            var filter = new StopWordsFilter(stopwords);
            var input = new List<string> { "Abc", "of", "cba", "IN", "the", "car" };
            stopwords.Remove("iN");
            filter.Filter(input).Value.Should().BeEquivalentTo(new List<string> { "abc", "in", "cba", "car" });
        }

        [Test]
        public void FontSizeByCount_CalculatesFontSizeCorrectly()
        {
            var fontSizeByCount = new FontSizeByCount();
            var input = new List<string> { "abc", "abc", "abc", "ab", "ab", "a" };
            var wordsWithFont = fontSizeByCount.CalculateFontSize(input, new FontFamily("Arial")).Value.ToList();
            wordsWithFont[0].Font.Size.Should().Be(35);
            wordsWithFont[1].Font.Size.Should().Be(22.5f);
            wordsWithFont[2].Font.Size.Should().Be(10);
        }


        [TestCase(100)]
        [TestCase(500)]
        [TestCase(1000)]
        public void TagCloudCreator_DrawsImageWithCurrectSize(int size)
        {
            var project_path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(
                Path.GetDirectoryName(Directory.GetCurrentDirectory()))), "TagsCloudContainer");
            var creator = GetCreator();
            creator.SetImageSize(size);
            creator.Create(Path.Combine(project_path, "input.txt"),
                project_path, "TestCloud");

            var imagePath = Path.Combine(project_path, "TestCloud.png");
            File.Exists(imagePath).Should().BeTrue();
            using (var image = Image.FromFile(imagePath))
            {
                image.Size.Should().BeEquivalentTo(new Size(size, size));
            }

            File.Delete(imagePath);
        }

        [Test]
        public void TagCloudCreator_RewriteExistedImage()
        {
            var project_path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(
                Path.GetDirectoryName(Directory.GetCurrentDirectory()))), "TagsCloudContainer");
            var creator = GetCreator();

            creator.SetImageSize(100);
            creator.Create(Path.Combine(project_path, "input.txt"),
                project_path, "TestCloud");

            creator.SetImageSize(200);
            creator.Create(Path.Combine(project_path, "input.txt"),
                project_path, "TestCloud");

            var imagePath = Path.Combine(project_path, "TestCloud.png");
            File.Exists(imagePath).Should().BeTrue();
            using (var image = Image.FromFile(imagePath))
            {
                image.Size.Should().BeEquivalentTo(new Size(200, 200));
            }
            File.Delete(imagePath);
        }

        private TagsCloudCreator GetCreator()
        {
            var scope = Configurator.GetContainer().Value.BeginLifetimeScope();
            return scope.Resolve<TagsCloudCreator>();
        }
    }
}
