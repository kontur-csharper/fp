using System.Collections.Generic;
using System.Drawing;
using TagsCloud.TagsCloudProcessing;
using TagsCloud.TextProcessing.WordsConfig;

namespace TagsCloud.ImageProcessing.ImageBuilders
{
    public class ImageBuilder : IImageBuilder
    {
        private readonly IWordConfig wordsConfig;

        public ImageBuilder(IWordConfig wordsConfig)
        {
            this.wordsConfig = wordsConfig;
        }

        public void DrawTags(IEnumerable<Tag> tags, Bitmap bitmap)
        {
            using var graphics = Graphics.FromImage(bitmap);
            using var brush = new SolidBrush(wordsConfig.Color);

            foreach (var tag in tags)
            {
                using var font = new Font(tag.FontConfig.FontFamily, tag.FontConfig.Size);
                graphics.DrawString(tag.Value, font, brush, tag.Rectangle);
            }
        }
    }
}
