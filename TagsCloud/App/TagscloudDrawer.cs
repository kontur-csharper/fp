﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public class TagsCloudDrawer : ITagsCloudDrawer
    {
        private const int MaxFontSize = 100;
        private readonly TagsCloudSettings tagsCloudSettings;
        private IRectanglesLayouter layouter;

        public TagsCloudDrawer(IRectanglesLayouter layouter, TagsCloudSettings tagsCloudSettings)
        {
            this.layouter = layouter;
            this.tagsCloudSettings = tagsCloudSettings;
        }

        public Result<Image> GetTagsCloud(IEnumerable<Word> words)
        {
            if (words == null)
                return Result.Fail<Image>("Words collection should not be null");
            var image = new Bitmap(tagsCloudSettings.ImageSize.Width, tagsCloudSettings.ImageSize.Height);
            var graphics = Graphics.FromImage(image);
            var tagsGetResult = GetTagsFromWords(
                words
                    .OrderBy(word => -word.Weight)
                    .Take(tagsCloudSettings.MaxWordsCount),
                graphics);
            if (!tagsGetResult.IsSuccess)
                return Result.Fail<Image>(tagsGetResult.Error);
            var tags = tagsGetResult.Value;
            var cloudBounds = CalculateTagsCloudBounds(tags.Select(tag => tag.Rectangle));
            var cloudExpectedSize = tagsCloudSettings.ImageSize * tagsCloudSettings.CloudToImageScaleRatio;
            var cloudSizeRatio = CalculateRatio(cloudBounds.Size, cloudExpectedSize);
            tags = tags.Select(tag => tag.RescaleTag(cloudSizeRatio)).ToArray();
            var layouterCenterDelta = new Size(cloudBounds.X + cloudBounds.Width / 2,
                cloudBounds.Y + cloudBounds.Height / 2) * cloudSizeRatio;
            var imageCenter = new PointF(
                (float) tagsCloudSettings.ImageSize.Width / 2,
                (float) tagsCloudSettings.ImageSize.Height / 2);
            DrawTagsCloud(tags, imageCenter - layouterCenterDelta, graphics);
            return Result.Ok((Image) image);
        }


        public void SetNewLayouter(IRectanglesLayouter newLayouter)
        {
            layouter = newLayouter;
        }

        private Result<Tag[]> GetTagsFromWords(IEnumerable<Word> words, Graphics graphics)
        {
            if (words == null)
                return Result.Fail<Tag[]>("Words collection should not be null.");
            var tagsArrayResult = Result.Of(() =>
            {
                return words
                    .Select(word =>
                    {
                        var wordFontSize = Math.Max((int) (word.Weight * MaxFontSize), 1);
                        var wordFont = new Font(tagsCloudSettings.CurrentFontFamily,
                            wordFontSize,
                            tagsCloudSettings.CurrentFontStyle);
                        var newRectangle =
                            layouter.PutNextRectangle(CalculateWordsSize(word.Value, wordFont, graphics));
                        return new Tag(word.Value, wordFont, newRectangle.Value);
                    })
                    .ToArray();
            });

            layouter.Reset();
            return tagsArrayResult;
        }

        private static float CalculateRatio(Size tagsCloudSize, ImageSize imageSize)
        {
            if ((double) tagsCloudSize.Width / imageSize.Width > (float) tagsCloudSize.Height / imageSize.Height)
                return (float) imageSize.Width / tagsCloudSize.Width;
            return (float) imageSize.Height / tagsCloudSize.Height;
        }

        public Result<None> DrawTagsCloud(IEnumerable<Tag> tags, PointF center, Graphics graphics)
        {
            if (tags == null)
                return Result.Fail<None>("Tags collection should not be null.");
            graphics.TranslateTransform(center.X, center.Y);
            graphics.Clear(tagsCloudSettings.Palette.BackgroundColor);
            var brush = new SolidBrush(tagsCloudSettings.Palette.PrimaryColor);
            foreach (var word in tags)
                graphics.DrawString(word.Value,
                    word.Font,
                    brush,
                    word.Rectangle.Location);
            return Result.Ok();
        }

        private Rectangle CalculateTagsCloudBounds(IEnumerable<Rectangle> rectangles)
        {
            var maxX = 0;
            var maxY = 0;
            var minX = 0;
            var minY = 0;
            foreach (var rectangle in rectangles)
            {
                maxX = Math.Max(maxX, rectangle.Location.X + rectangle.Width);
                maxY = Math.Max(maxY, rectangle.Location.Y + rectangle.Height);
                minX = Math.Min(minX, rectangle.Location.X);
                minY = Math.Min(minY, rectangle.Location.Y);
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        private Size CalculateWordsSize(string word, Font font, Graphics graphics)
        {
            var floatSize = graphics.MeasureString(word, font);
            return new Size(
                (int) Math.Ceiling(floatSize.Width),
                (int) Math.Ceiling(floatSize.Height));
        }
    }
}