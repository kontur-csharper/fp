using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using TagsCloud.GUI;
using TagsCloud.Infrastructure;
using TagsCloud.Layouters;
using TagsCloud.UiActions;
using TagsCloud.WordsProcessing;

namespace TagsCloud.Tests
{
    class InfrastructureTests
    {
        [Test]
        public void ImageHolderShould_ChangeLayouter_OnChangeActionCall()
        {
            var holder = A.Fake<IImageHolder>();
            var fakeLayouter = A.Fake<SpiralCloudLayouter>();
            var changeAlgorithmAction = new SelectSpiralLayouterAction(holder, fakeLayouter);
            changeAlgorithmAction.Perform();
            A.CallTo(() => holder.ChangeLayouter(fakeLayouter)).MustHaveHappenedOnceExactly();

        }

        [Test]
        public void LayouterShould_UpdateCenterPoint_WhenChangedResolution()
        {
            var layouter = A.Fake<ICloudLayouter>();
            var settings = A.Fake<ImageSettings>(options =>
                options.WithArgumentsForConstructor(new List<object?> { 1920, 1080 }));
            var parser = A.Fake<IWordsFrequencyParser>();

            var holder = A.Fake<IImageHolder>(o =>
                o.Wrapping(A.Fake<PictureBoxImageHolder>(opt =>
                    opt.WithArgumentsForConstructor(new List<object?> { parser, settings, layouter }))));

            settings.Width = 1720;
            holder.RecreateCanvas(settings);
            A.CallTo(() => layouter.UpdateCenterPoint(settings)).MustHaveHappened(4, Times.Exactly);
        }

        [Test]
        public void MainFormShould_AdjustResolution_ToStartSettings()
        {
            var layouter = A.Fake<ICloudLayouter>();
            var settings = A.Fake<ImageSettings>(options =>
                options.WithArgumentsForConstructor(new List<object?> {1920, 1080}));
            var parser = A.Fake<IWordsFrequencyParser>();
            var holder = new PictureBoxImageHolder(parser, settings, layouter);
            var mainForm = new MainForm(new IUiAction[0], holder);
            mainForm.ClientSize.Should().Be(new Size(settings.Width, settings.Height));
        }

        [Test]
        public void WordsFrequencyParserShould_CallToWordsFilter_WhenParsingWords()
        {
            var path = Assembly.GetExecutingAssembly().Location + "testText.txt";
            var words = new List<string> { "one", "two", "three" };

            var filter = A.Fake<IWordsFilter>();
            var parser = A.Fake<IWordsFrequencyParser>(opts => 
                opts.Wrapping(new WordsFrequencyParser(filter)));

            File.WriteAllText(path, string.Join(Environment.NewLine, words));
            parser.ParseWordsFrequencyFromFile(path);
            A.CallTo(() => filter.GetCorrectWords(words)).WithAnyArguments().MustHaveHappenedOnceExactly();
        }

        [Test]
        public void BigTest()
        {
            var path = Assembly.GetExecutingAssembly().Location + "testText.txt";
            File.WriteAllText(path, GetText());

            var configurator = new ExcludingWordsConfigurator(new HashSet<string>());
            var filter = new WordsFilter(configurator);
            var parser = new WordsFrequencyParser(filter);
            var fakeParser = A.Fake<IWordsFrequencyParser>(opts => opts.Wrapping(parser));
            var settings = A.Fake<ImageSettings>(opts => 
                opts.WithArgumentsForConstructor(new List<object?> {1920, 1080}));
            var layouter = A.Fake<ICloudLayouter>();
            var holder = new PictureBoxImageHolder(fakeParser, settings, layouter);

            holder.RenderWordsFromFile(path);
            A.CallTo(() => fakeParser.ParseWordsFrequencyFromFile(path)).MustHaveHappenedOnceExactly();
            A.CallTo(() => layouter.PutNextRectangle(Size.Empty)).WithAnyArguments()
                .MustHaveHappened(3, Times.Exactly);

        }

        [Test]
        public void ImageHolder_RenderWordsFromFile_Should_Fail_WhenFileContentNotCorrect()
        {
            var filter = new WordsFilter(new ExcludingWordsConfigurator(new List<string>()));
            var parser = new WordsFrequencyParser(filter);
            var settings = new ImageSettings(1920, 1080);
            var holder = new PictureBoxImageHolder(parser, settings, new CircularCloudLayouter(settings));

            var path = Assembly.GetExecutingAssembly().Location + "testText.txt";
            File.WriteAllText(path, GetText() + $"{Environment.NewLine} not one word");

            var result = holder.RenderWordsFromFile(path);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Входной файл должен содержать только одно слово в строке");
        }

        [Test]
        public void ImageHolder_RenderWordsFromFile_Should_Fail_WhenIncorrectFilePath()
        {
            var filter = new WordsFilter(new ExcludingWordsConfigurator(new List<string>()));
            var parser = new WordsFrequencyParser(filter);
            var settings = new ImageSettings(1920, 1080);
            var holder = new PictureBoxImageHolder(parser, settings, new CircularCloudLayouter(settings));

            var result = holder.RenderWordsFromFile("");
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Запрошенный файл не найден");
        }
        
        [Test]
        public void ImageHolder_SaveImage_Should_Fail_WithIncorrectFormat()
        {
            var filter = new WordsFilter(new ExcludingWordsConfigurator(new List<string>()));
            var parser = new WordsFrequencyParser(filter);
            var settings = new ImageSettings(1920, 1080);
            var holder = new PictureBoxImageHolder(parser, settings, new CircularCloudLayouter(settings));

            var result = holder.SaveImage("C:\\");
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Неподдерживаемое расширение файла");
        }

        private string GetText()
        {
            var words = new List<string>();
            for (var i = 0; i < 40; i++)
                words.Add("first");
            for (var i = 0; i < 20; i++)
                words.Add("second");
            for (var i = 0; i < 5; i++)
                words.Add("third");
            return string.Join(Environment.NewLine, words);
        }
    }
}
