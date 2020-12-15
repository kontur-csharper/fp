using System.Drawing;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.Settings
{
    public class Palette : IPaletteSettingsHolder
    {
        public static readonly Color DefaultTextColor = Color.White;
        public static readonly Color DefaultBackgroundColor = Color.Black;

        public static readonly Palette Instance = new Palette();

        private Palette()
        {
            SetDefault();
        }

        public Color TextColor { get; set; }
        public Color BackgroundColor { get; set; }

        public void SetDefault()
        {
            TextColor = DefaultTextColor;
            BackgroundColor = DefaultBackgroundColor;
        }
    }
}