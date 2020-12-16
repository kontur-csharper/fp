using System;
using FluentAssertions;
using NUnit.Framework;
using TagCloud.TextProcessing;

namespace TagCloud.Tests
{
    public class TextReaderTests
    {
        private PathCreator pathCreator = new PathCreator();
        private TxtTextReader txtReader = new TxtTextReader();
        private DocxTextReader docxReader = new DocxTextReader();
        
        [Test]
        public void TxtTextReader_ReadStrings_OnCorrectFile()
        {
            var readingResult = txtReader.ReadStrings(pathCreator.GetPathToFile("input.txt"));
            readingResult.IsSuccess.Should().BeTrue();
            readingResult.Value.Should().HaveCount(24)
                .And.Contain("кошка")
                .And.Contain("Кошка")
                .And.Contain("cat")
                .And.Contain("Андрей")
                .And.Contain("крокодил");
        }
        [Test]
        public void TxtTextReader_ReadStrings_ReturnCorrectFail_OnDirectory()
        {
            var path = pathCreator.GetCurrentPath();
            var readingResult = txtReader.ReadStrings(path);
            readingResult.IsSuccess.Should().BeFalse();
            readingResult.Error.Should().Contain(path).And.Contain("Is directory");
        }

        [Test]
        public void TxtTextReader_ReadStrings_ReturnCorrectFail_OnWrongDirectory()
        {
            var path = pathCreator.GetCurrentPath();
            var readingResult = txtReader.ReadStrings(path + "asdf\\input.txt");
            readingResult.IsSuccess.Should().BeFalse();
            readingResult.Error.Should().Contain(path).And.Contain("Directory not found");
        }
        
        [Test]
        public void DocxTextReader_ReadStrings_OnCorrectFile()
        {
            var readingResult = docxReader.ReadStrings(pathCreator.GetPathToFile("input.docx"));
            Console.WriteLine(readingResult.Error);
            readingResult.IsSuccess.Should().BeTrue();
            readingResult.Value.Should().HaveCount(25)
                .And.Contain("кошка")
                .And.Contain("Кошка")
                .And.Contain("cat")
                .And.Contain("Андрей")
                .And.Contain("крокодил");
        }
        
        [Test]
        public void DocxTextReader_ReadStrings_ReturnCorrectFail_OnDirectory()
        {
            var path = pathCreator.GetCurrentPath();
            var readingResult = docxReader.ReadStrings(path);
            readingResult.IsSuccess.Should().BeFalse();
            readingResult.Error.Should().Contain(path).And.Contain("Is directory");
        }

        [Test]
        public void DocxTextReader_ReadStrings_ReturnCorrectFail_OnWrongDirectory()
        {
            var path = pathCreator.GetCurrentPath();
            var readingResult = docxReader.ReadStrings(path + "asdf\\input.docx");
            readingResult.IsSuccess.Should().BeFalse();
            readingResult.Error.Should().Contain(path).And.Contain("Directory not found");
        }
    }
}