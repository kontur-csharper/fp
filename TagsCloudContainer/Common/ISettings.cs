namespace TagsCloudContainer.Common
{
    public interface ISettings
    {
        Result<ISettings> CheckSettings();
    }
}