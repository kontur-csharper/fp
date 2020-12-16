using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using RectanglesCloudLayouter.CalculatorForCloudRadius;
using RectanglesCloudLayouter.LayouterOfRectangles;
using RectanglesCloudLayouter.SettingsForSpiral;
using RectanglesCloudLayouter.SpiralAlgorithm;
using TagsCloud.Settings.SettingsForTextProcessing;
using TagsCloud.TagsLayouter;
using TagsCloud.TextProcessing.FrequencyOfWords;
using TagsCloud.TextProcessing.ParserForWordsAndSpeechParts;
using TagsCloud.TextProcessing.Tags;
using TagsCloud.TextProcessing.TextConverters;
using TagsCloud.TextProcessing.TextFilters;
using TagsCloud.TextProcessing.WordsMeasurer;

namespace TagsCloudTests.FunctionalTests
{
    public class WordTagsLayouterFunctionalTest
    {
        private WordTagsLayouter _sut;
        private static Font _font = new Font("arial", 5);

        [SetUp]
        public void SetUp()
        {
            var frequency = new WordsFrequency(
                new MyStemConverter(Path.GetFullPath("mystem.exe").Replace($"Tests", ""), "-ni"),
                new List<IWordsFilter>
                {
                    new SpeechPartsFilter(
                        new NormalizedWordAndSpeechPartParser(),
                        new[] {"PR", "PART", "INTJ", "CONJ", "ADVPRO", "APRO", "NUM", "SPRO"}),
                    new WordsFilter(new TextProcessingSettings(new[] {"велосипед", "осень"}))
                });
            var rectanglesLayouter = new RectanglesLayouter(
                new ArchimedeanSpiral(new Point(0, 0), new SpiralSettings(1, 0.5)),
                new CloudRadiusCalculator());
            var wordMeasurer = new WordMeasurer();

            _sut = new WordTagsLayouter(frequency, rectanglesLayouter, wordMeasurer, _font);
        }

        [Test]
        public void GetWordTagsAndCloudRadius_TagsAndRadius_WhenManyWords()
        {
            var text = "велосипед выход / осень \r\n.. он под";
            var expectedWord = text.Split()[1];
            var expectedFont = new Font(_font.FontFamily, _font.Size + (float) Math.Log2(1) * 7);
            var sizeF = Graphics.FromHwnd(IntPtr.Zero).MeasureString(expectedWord, expectedFont);
            var expectedSize = new Size((int) Math.Ceiling(sizeF.Width), (int) Math.Ceiling(sizeF.Height));
            var expectedResult = new List<WordTag>
                {new WordTag(expectedWord, new Rectangle(Point.Empty - expectedSize / 2, expectedSize), expectedFont)};

            var act = _sut.GetWordTagsAndCloudRadius(text);

            act.GetValueOrThrow().Item1.Should().BeEquivalentTo(expectedResult);
            act.GetValueOrThrow().Item2.Should().BeGreaterThan(expectedSize.Width / 2);
        }
    }
}