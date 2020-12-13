using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloud.Layouter;
using TagsCloud.Options;
using TagsCloud.ResultOf;

namespace TagsCloud.Drawer
{
    public class RectangleLayout : IRectangleLayout
    {
        private readonly ILayouter layouter;
        private readonly ILayoutDrawer drawer;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private readonly IImageOptions imageOptions;
        private readonly IFontOptions fontOptions;

        public RectangleLayout(ILayouter layouter, ILayoutDrawer drawer, IImageOptions imageOptions,
            IFontOptions fontOptions)
        {
            this.layouter = layouter;
            this.drawer = drawer;
            this.imageOptions = imageOptions;
            this.fontOptions = fontOptions;
            bitmap = new Bitmap(imageOptions.Width, imageOptions.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public Result<None> PlaceWords(Dictionary<string, int> words) =>
            Result.Of(() => MakeTags(words))
                .Then(PlaceTags)
                .Then(TagsInsideImage)
                .Then(tags => drawer.AddTags(tags));

        private IEnumerable<Tag> MakeTags(Dictionary<string, int> words)
        {
            var tags = new List<Tag>();
            foreach (var (word, count) in words)
                tags.Add(new Tag(word, CalculateFontSize(count), fontOptions.FontFamily, fontOptions.FontColor));

            return tags;
        }

        private IEnumerable<Tag> PlaceTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                var tagSize = GetTagSize(tag);
                tag.Rectangle = layouter.PutNextRectangle(tagSize);
            }

            return tags;
        }

        private Size GetTagSize(Tag tag) =>
            graphics.MeasureString(tag.Text, new Font(tag.FontFamily, tag.FontSize)).ToSize();

        private static int CalculateFontSize(int wordCount) => (int) (Math.Log(Math.Max(wordCount, 6), 6) * 10);

        public void DrawLayout() => drawer.Draw(graphics);

        public void SaveLayout()
        {
            var outputDirectory = imageOptions.ImageOutputDirectory ?? Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(outputDirectory, imageOptions.ImageName + imageOptions.ImageExtension);
            bitmap.Save(fullPath);
            Console.WriteLine($"Tag cloud visualization saved to file {fullPath}.");
        }

        private Result<IEnumerable<Tag>> TagsInsideImage(IEnumerable<Tag> tags) =>
            tags.Any(tag => RectangleInsideImage(tag.Rectangle))
                ? Result.Fail<IEnumerable<Tag>>("Some tags are outside of the image. Change image size.")
                : Result.Ok(tags);

        private bool RectangleInsideImage(Rectangle rectangle) =>
            imageOptions.Width < rectangle.Right || 0 > rectangle.Left ||
            imageOptions.Height < rectangle.Bottom || 0 > rectangle.Top;
    }
}