using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResultOf;
using TagCloud.Layout;
using TagCloud.TextProcessing;

namespace TagCloud
{
    public class TagsCreator: ITagsCreator
    {
        private readonly IFrequencyAnalyzer frequencyAnalyzer;
        private readonly ILayouter layouter;
        private const double heightCoefficient = 8;
        private const double widthCoefficient = 0.7;
        
        public TagsCreator(IFrequencyAnalyzer frequencyAnalyzer, ILayouter layouter)
        {
            this.frequencyAnalyzer = frequencyAnalyzer;
            this.layouter = layouter;
        }
        
        public Result<List<Tuple<string, Rectangle>>> GetTags(string filename, int canvasHeight)
            => frequencyAnalyzer.GetFrequencyDictionary(filename)
                .Then(frequencies =>
                frequencies
                    .OrderByDescending(pair => pair.Value)
                    .Select(pair => GetTag(pair, canvasHeight))
                    .ToList());

        private Tuple<string, Rectangle> GetTag(KeyValuePair<string, double> pair, int canvasHeight)
        {
            var frequency = pair.Value;
            var tagString = pair.Key;
            var height = (int) Math.Round(canvasHeight * Math.Sqrt(frequency / heightCoefficient));
            var width = (int)Math.Round((double)height * (tagString.Length) * widthCoefficient);
            var rectangle = layouter.PutNextRectangle(new Size(width, height));
            return new Tuple<string, Rectangle>(tagString, rectangle);
        }
    }
}