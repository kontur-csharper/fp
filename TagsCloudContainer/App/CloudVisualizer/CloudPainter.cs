using System.Collections.Generic;
using System.Drawing;
using ResultOf;
using TagsCloudContainer.App.CloudGenerator;
using TagsCloudContainer.Infrastructure.CloudVisualizer;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.CloudVisualizer
{
    public class CloudPainter : ICloudPainter
    {
        private readonly IPaletteSettingsHolder palette;
        private readonly IFontSettingsHolder fontSettings;
        private readonly IImageSizeSettingsHolder sizeSettings;

        public CloudPainter(IPaletteSettingsHolder palette, IFontSettingsHolder fontSettings, 
            IImageSizeSettingsHolder sizeSettings)
        {
            this.palette = palette;
            this.fontSettings = fontSettings;
            this.sizeSettings = sizeSettings;
        }

        public Result<None> Paint(IEnumerable<Tag> cloud, Graphics g)
        {
            using var backgroundBush = new SolidBrush(palette.BackgroundColor);
            g.FillRectangle(backgroundBush, 0, 0, sizeSettings.Width, sizeSettings.Height);
            using var textBrush = new SolidBrush(palette.TextColor);
            foreach (var tag in cloud)
            {
                var validationResult = ValidateTagIsNotOutOfImageBounds(tag);
                if (!validationResult.IsSuccess)
                    return Result.Fail<None>(validationResult.Error);
                var font = fontSettings.Font;
                g.DrawString(tag.Word, new Font(font.FontFamily,
                        (float) tag.FontSize, font.Style,
                        font.Unit, font.GdiCharSet),
                    textBrush, tag.Rectangle.Location);
            }

            return new Result<None>();
        }

        private Result<Tag> ValidateTagIsNotOutOfImageBounds(Tag tag)
        {
            var rect = tag.Rectangle;
            var imageWidth = sizeSettings.Width;
            var imageHeight = sizeSettings.Height;
            if (CheckNumberIsOutOfRange(rect.Top, 0, imageHeight) ||
                CheckNumberIsOutOfRange(rect.Bottom, 0, imageHeight) ||
                CheckNumberIsOutOfRange(rect.Right, 0, imageWidth) ||
                CheckNumberIsOutOfRange(rect.Left, 0, imageWidth))
                return Result.Fail<Tag>("Tag is out of image bounds");
            return Result.Ok(tag);
        }

        private bool CheckNumberIsOutOfRange(int number, int start, int end)
        {
            return number < start || number > end;
        }
    }
}