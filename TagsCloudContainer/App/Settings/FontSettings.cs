using System.Drawing;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.Settings
{
    public class FontSettings : IFontSettingsHolder
    {
        public static readonly Font DefaultFont = new Font("Arial", 10);

        public static readonly FontSettings Instance = new FontSettings();

        private FontSettings()
        {
            SetDefault();
        }

        public Font Font { get; set; }

        public void SetDefault()
        {
            Font = DefaultFont;
        }
    }
}