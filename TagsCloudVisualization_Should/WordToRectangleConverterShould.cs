using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.CloudTags;
using TagsCloudVisualization.Configs;
using TagsCloudVisualization.PointProviders;
using TagsCloudVisualization.WordsConverters;

namespace TagsCloudVisualization_Should
{
    public class WordToRectangleConverterShould
    {
        [Test]
        public void ConvertWords_CorrectListWithTags_NormalWords()
        {
            var config = new Config();
            config.SetValues(new Font(FontFamily.GenericMonospace, 25),
                new Point(1500, 1500), Color.Blue, new Size(1500, 1500), new HashSet<string>());
            var pointProvider = new PointProvider(config);
            var cloud = new CircularCloudLayouter(pointProvider);
            var converter = new WordsToCloudTagConverter(cloud, config);
            var words = new List<string> {"он", "пошел"};
            var expectedTags = new List<ICloudTag>
            {
                new CloudTag(new Rectangle(1500, 1500, 52, 41), "он"),
                new CloudTag(new Rectangle(1500, 1459, 114, 41), "пошел")
            };

            var actualTags = converter.ConvertWords(words);

            actualTags.GetValueOrThrow().Should().Equal(expectedTags, (c1, c2) => c1.Equals(c2));
        }
    }
}