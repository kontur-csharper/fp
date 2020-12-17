namespace TagCloud.Infrastructure.Settings.SettingsProviders
{
    public interface IWordCountThresholdSettingProvider
    {
        public int WordCountThreshold { get; }
    }
}