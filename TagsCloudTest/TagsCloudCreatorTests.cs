using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ImageProcessing.Config;
using TagsCloud.Layouter;
using TagsCloud.TagsCloudProcessing;
using TagsCloud.TagsCloudProcessing.TagsGenerators;
using TagsCloud.TextProcessing.Converters;
using TagsCloud.TextProcessing.TextFilters;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloudTest
{
    public class TagsCloudCreatorTests
    {
        private TagsCloudCreator tagsCloudCreator;
        private WordConfig wordsConfig;
        private ImageConfig imageConfig;
        private readonly ServiceProvider container = ContainerBuilder.BuildContainer();

        private string textPath;
        private string imagePath;
        private string expectedImagePath;

        [SetUp]
        public void SetUp()
        {
            var path = TestContext.CurrentContext.TestDirectory;

            textPath = Path.Combine(path, "Resources", "text.txt");
            imagePath = Path.Combine(path, "Resources", "test.png");
            expectedImagePath = Path.Combine(path, "Resources", "expected.png");

            tagsCloudCreator = container.GetService<TagsCloudCreator>();

            wordsConfig = container.GetService<WordConfig>();
            ConfigureWordsConfig(textPath);

            imageConfig = container.GetService<ImageConfig>();
            ConfigureImageConfig(imagePath);
        }

        private void ConfigureWordsConfig(string path)
        {
            wordsConfig.Color = Color.Red;

            wordsConfig.ConvertersNames = container
                .GetService<IServiceFactory<IWordConverter>>()
                .GetServiceNames().ToArray();

            wordsConfig.FilersNames = container
                .GetService<IServiceFactory<ITextFilter>>()
                .GetServiceNames().ToArray();

            wordsConfig.Font = new Font(FontFamily.GenericMonospace, 20);

            wordsConfig.LayouterName = container
                .GetService<IServiceFactory<IRectanglesLayouter>>()
                .GetServiceNames().First();

            wordsConfig.TagGeneratorName = container
                .GetService<IServiceFactory<ITagsGenerator>>()
                .GetServiceNames().First();

            wordsConfig.Path = path;
        }

        private void ConfigureImageConfig(string path)
        {
            imageConfig.ImageSize = new Size(1000, 1000);
            imageConfig.Path = path;
        }

        [Test]
        public void CreateCloudFunctionalTest()
        {
            tagsCloudCreator.CreateCloud(textPath, imagePath);

            var current = File.ReadAllBytes(imagePath);
            var expected = File.ReadAllBytes(expectedImagePath);

            current.Should().Equal(expected);
        }

        [Test]
        public void CreateCloudShouldHandleInvalidTextPathWhenExtensionNotSupported()
        {
            var path = imagePath;
            wordsConfig.Path = path;
            var result = tagsCloudCreator.CreateCloud(path, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("file type .png is not supported");
        }

        [Test]
        public void CreateCloudShouldHandleInvalidTextPathWhenFileNotExist()
        {
            var path = textPath.Replace("text.txt", "abc.txt");
            wordsConfig.Path = path;
            var result = tagsCloudCreator.CreateCloud(path, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("Path: " + path + " don't exists");
        }

        [Test]
        public void CreateCloudShouldHandleInvalidImagePathExtensionNotExist()
        {
            var path = imagePath.Replace(".png", ".ff");
            imageConfig.Path = path;
            var result = tagsCloudCreator.CreateCloud(textPath, path);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("image type .ff is not supported");
        }

        [Test]
        public void CreateCloudShouldHandleInvalidLayouterName()
        {
            wordsConfig.LayouterName = "fffff";
            var result = tagsCloudCreator.CreateCloud(textPath, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("layouter fffff not supported");
        }

        [Test]
        public void CreateCloudShouldHandleInvalidTagsGeneratorName()
        {
            wordsConfig.TagGeneratorName = "fffff";
            var result = tagsCloudCreator.CreateCloud(textPath, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("tags generator fffff not supported");
        }

        [Test]
        public void CreateCloudShouldHandleInvalidFilterNames()
        {
            wordsConfig.FilersNames = new[] { "fff" };
            var result = tagsCloudCreator.CreateCloud(textPath, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("filter fff not supported");
        }

        [Test]
        public void CreateCloudShouldHandleInvalidConverterNames()
        {
            wordsConfig.ConvertersNames = new[] { "fff" };
            var result = tagsCloudCreator.CreateCloud(textPath, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("converter fff not supported");
        }

        [Test]
        public void CreateCloudShouldHandleInvalidFontName()
        {
            wordsConfig.Font = new Font("123ac", 15f);
            var result = tagsCloudCreator.CreateCloud(textPath, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("font 123ac not supported");
        }

        [Test]
        public void CreateCloudShouldHandleCloudOutOfImage()
        {
            imageConfig.ImageSize = new Size(10, 10);
            var result = tagsCloudCreator.CreateCloud(textPath, imagePath);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain("Cloud out of size image");
        }
    }
}
