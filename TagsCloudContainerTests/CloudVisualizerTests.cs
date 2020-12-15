using System.IO;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TagsCloudContainer.App;
using TagsCloudContainer.App.Settings;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Infrastructure.CloudVisualizer;

namespace TagsCloudContainerTests
{
    internal class CloudVisualizerTests
    {
        private readonly ICloudVisualizer visualizer;
        private readonly InputSettings inputSettings;
        private readonly ImageSizeSettings imageSizeSettings;

        public CloudVisualizerTests()
        {
            var serviceProvider = Program.GetAppServiceProvider();
            serviceProvider.GetRequiredService<IImageHolder>().RecreateImage();
            visualizer = serviceProvider.GetService<ICloudVisualizer>();
            inputSettings = InputSettings.Instance;
            imageSizeSettings = ImageSizeSettings.Instance;
        }

        [SetUp]
        public void SetUp()
        {
            inputSettings.SetDefault();
            imageSizeSettings.SetDefault();
            FilteringWordsSettings.Instance.SetDefault();
            FontSettings.Instance.SetDefault();
            Palette.Instance.SetDefault();
        }

        [Test]
        public void CloudVisualizer_ShouldVisualizeSuccessfully()
        {
            var result = visualizer.Visualize();
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public void CloudVisualizer_ShouldReturnResultWithError_IfInputFileIsNotFound()
        {
            inputSettings.InputFileName = "notExistedPath.txt";
            var expectedError = "Input file is not found";

            var result = visualizer.Visualize();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo(expectedError);
        }

        [Test]
        public void CloudVisualizer_ShouldReturnResultWithError_IfCannotReadInputFile()
        {
            inputSettings.InputFileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                "files", 
                "notDocxFileWithDocxExtension.docx");
            var expectedError = "Can't read input file";

            var result = visualizer.Visualize();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Contain(expectedError);
        }

        [Test]
        public void CloudVisualizer_ShouldReturnResultWithError_IfTextContainsWrongWord()
        {
            inputSettings.InputFileName = Path.Combine(
                Directory.GetCurrentDirectory(),
                "files",
                "textWithNonExistentWord.txt");
            var expectedError = "Can't normalize word рпмсппимпсапсвръъъъъ to initial form";

            var result = visualizer.Visualize();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo(expectedError);
        }

        [Test]
        public void CloudVisualizer_ShouldReturnResultWithError_IfTagsAreInOutImageBounds()
        {
            imageSizeSettings.Width = 10;
            imageSizeSettings.Height = 10;
            var expectedError = "Tag is out of image bounds";

            var result = visualizer.Visualize();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}
