using System.Drawing.Imaging;

namespace TagCloud.Infrastructure.Settings.SettingsProviders
{
    public interface IImageFormatSettingProvider
    {
        public ImageFormat Format { get; set; }
    }
}