using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer
{
    public class TagsCloudContainer_Result_Should
    {
        private TagsCloudContainer container;
        private WordsRendererToImageDebug renderer;
        private string basicWords = "some basic Words toTest";

        [Test]
        public void Fail_WhenFileWithWordsDoesNotExist()
        {
            var fileName = "Some_Not_Existing_File..."; 
            File.Exists(fileName).Should().BeFalse();
            
            container.AddFromFile(fileName);
            var result = container.Render();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().StartWith("Не удаётся прочитать слова из источника. ");
            result.ErrorMessage.Should().Contain($"Не удаётся прочитать файл {fileName}");
        }

        [Test]
        public void Fail_WhenPreprocessingFunctionThrowsSomething()
        {
            var result = container
                .AddFromText(basicWords)
                .Preprocessing(word => throw new Exception($"!hello, {word}"))
                .Render();
            
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().StartWith("Не удалось преобразовать слово. ");
            result.ErrorMessage.Should().Contain("!hello, ");
        }

        [Test]
        public void Fail_WhenRectanglesPlacedOutsideImageBorders()
        {
            renderer.AutoSize = false;
            renderer.Output = new Bitmap(600, 600);
            container.AddFromText("SuperMegaVeryLongLongLongWord");
            renderer.WithScale(((info, word) => 250));
            var result = container.Render();

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().StartWith("Прямоугольники выходят за границы изображения");
        }
        
        [SetUp]
        public void SetUp()
        {
            renderer = new WordsRendererToImageDebug();
            container = new TagsCloudContainer()
                .Rendering(renderer);
        }

        private static TextBuilder GetRandomText(params string[][] words) => new TextBuilder(words);
        private string[] regularWords => TextBuilder.regularWords;
        private string[] wordsToExclude => TextBuilder.wordsToExclude;
    }
}