namespace TagCloud.Infrastructure.Settings.SettingsProviders
{
    public interface IImageSettingsProvider : IFontSettingProvider
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string ImagePath { get; set; }
    }
}