using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ResultOf;
using TagsCloudContainer.Infrastructure.CloudGenerator;

namespace TagsCloudContainer.App.CloudGenerator
{
    internal class CloudGenerator : ICloudGenerator
    {
        private readonly IFontGetter fontGetter;
        private readonly ICloudLayouterFactory layouterFactory;

        public CloudGenerator(IFontGetter fontGetter, ICloudLayouterFactory layouterFactory)
        {
            this.fontGetter = fontGetter;
            this.layouterFactory = layouterFactory;
        }

        public Result<IEnumerable<Tag>> GenerateCloud(Dictionary<string, double> frequencyDictionary)
        {
            return layouterFactory
                .CreateCloudLayouter()
                .Then(layouter => GenerateTags(layouter, frequencyDictionary));
        }

        private IEnumerable<Tag> GenerateTags(ICloudLayouter layouter, 
            Dictionary<string, double> frequencyDictionary)
        {
            foreach (var pair in frequencyDictionary.OrderByDescending(pair => pair.Value))
            {
                var (word, frequency) = (pair.Key, pair.Value);
                var font = fontGetter.GetFont(word, frequency);
                var rectangleSize = GetRectangleSize(word, font);
                var nextRectangle = layouter.PutNextRectangle(rectangleSize);
                yield return new Tag(word, font.Size, nextRectangle);
            }
        }

        private Size GetRectangleSize(string word, Font font)
        {
            return TextRenderer.MeasureText(word, font);
        }
    }
}