using System;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer;

namespace TagsCloudContainerTests
{
    [TestFixture]
    public class AppSettingsValidatorTests
    {
        private AppSettings settings;
        private AppSettingsValidator validator;
        
        [SetUp]
        public void SetUp()
        {
            settings = new AppSettings();
            validator = new AppSettingsValidator(settings);
        }

        [Test]
        public void ValidateTextFilePath_IfFileDoesNotExist_ThrowArgumentException()
        {
            settings.PathToFile = "blabla.txt";
            Action act = () => validator.ValidateTextFilePath(new[] {".txt"});

            act.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void ValidateTextFile_IfFileFormatIsNotSupported_ThrowArgumentException()
        {
            settings.PathToFile = "../../../TestFiles/test.docx";
            Action act = () => validator.ValidateTextFilePath(new[] {".txt"});

            act.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void ValidatePartsOfSpeech_IfIncorrectPartsOfSpeechName_ThrowArgumentException()
        {
            settings.ExcludedPartsOfSpeechNames = new[] { "blabla" };
            Action act = () => validator.ValidatePartsOfSpeech();

            act.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void ValidateColors_IfIncorrectFontColorName_ThrowArgumentException()
        {
            settings.FontColorName = "i'm not a color";
            settings.BackgroundColorName = "White";
            Action act = () => validator.ValidateColors();

            act.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void ValidateColors_IfIncorrectBackgroundColorName_ThrowArgumentException()
        {
            settings.FontColorName = "Black";
            settings.BackgroundColorName = "i'm not a color";
            Action act = () => validator.ValidateColors();

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ValidateFont_IfIncorrectFontName_ThrowArgumentException()
        {
            settings.FontName = "abcdef";
            Action act = () => validator.ValidateFont();

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ValidateImagePath_IfFormatIsNotSupported_ThrowArgumentException()
        {
            settings.ImagePath = "image.gpn";
            Action act = () => validator.ValidateImagePath(new[] {".png"});

            act.Should().Throw<ArgumentException>();
        }
    }
}