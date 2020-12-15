using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.App.DataReader;
using TagsCloudContainer.App.Settings;
using TagsCloudContainer.Infrastructure.DataReader;

namespace TagsCloudContainerTests
{
    internal class DataReaderFactoryTests
    {
        private readonly IDataReaderFactory dataReaderFactory;

        private readonly string docxFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.docx");

        private readonly string txtFilePath = Path.Combine(Directory.GetCurrentDirectory(),
            "files", "input.txt");

        private readonly InputSettings inputSettings;

        public DataReaderFactoryTests()
        {
            dataReaderFactory = new DataReaderFactory(InputSettings.Instance);
            inputSettings = InputSettings.Instance;
        }

        [Test]
        public void DataReaderFactory_ShouldCreateTxtReader()
        {
            DataReaderFactory_ShouldCreateReaderForFile(txtFilePath);
        }

        [Test]
        public void DataReaderFactory_ShouldCreateDocxReader()
        {
            DataReaderFactory_ShouldCreateReaderForFile(docxFilePath);
        }

        [Test]
        public void DataReaderFactory_ShouldNotCreateReaderForFile_IfInputFileIsWithInvalidExtension()
        {
            inputSettings.InputFileName = "file.png";
            var expectedError = "Unknown input file format";

            var result = dataReaderFactory.CreateDataReader();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo(expectedError);
        }

        private void DataReaderFactory_ShouldCreateReaderForFile(string filePath)
        {
            inputSettings.InputFileName = filePath;

            var result = dataReaderFactory.CreateDataReader();

            result.IsSuccess.Should().BeTrue();
        }
    }
}