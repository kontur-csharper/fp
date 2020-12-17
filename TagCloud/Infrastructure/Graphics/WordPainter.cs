using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.Infrastructure.Layout;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Graphics
{
    public class WordPainter : IPainter<string>
    {
        private readonly ColorPicker colorPicker;
        private readonly Func<IImageSettingsProvider> imageSettingsProvider;
        private readonly ILayouter<Size, Rectangle> layouter;

        public WordPainter(
            ILayouter<Size, Rectangle> layouter,
            Func<IImageSettingsProvider> imageSettingsProvider,
            ColorPicker colorPicker)
        {
            this.layouter = layouter;
            this.imageSettingsProvider = imageSettingsProvider;
            this.colorPicker = colorPicker;
        }

        public Image GetImage(IEnumerable<TokenInfo> tokens)
        {
            var settings = imageSettingsProvider();
            var image = new Bitmap(settings.Width, settings.Height);
            using var imageGraphics = System.Drawing.Graphics.FromImage(image);

            using (layouter)
            {
                foreach (var info in tokens)
                {
                    var hitbox = layouter.GetPlace(info.Size);
                    using var font = new Font(settings.FontFamily, info.FontSize);
                    using var brush = new SolidBrush(colorPicker.GetColor(info));
                    imageGraphics.DrawString(info.Token, font, brush, hitbox.Location);
                }
            }

            return image;
        }
    }
}