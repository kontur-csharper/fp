using System.ComponentModel;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.Settings
{
    public class ImageSizeSettings : IImageSizeSettingsHolder
    {
        public static readonly int DefaultWidth = 500;
        public static readonly int DefaultHeight = 500;

        public static readonly ImageSizeSettings Instance = new ImageSizeSettings();

        private ImageSizeSettings()
        {
            SetDefault();
        }

        [TypeConverter(typeof(CustomSizeConverter))]
        public int Width { get; set; }

        [TypeConverter(typeof(CustomSizeConverter))]
        public int Height { get; set; }

        public void SetDefault()
        {
            Width = DefaultWidth;
            Height = DefaultHeight;
        }
    }
}