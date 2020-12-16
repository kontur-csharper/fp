using System.Drawing.Imaging;

namespace TagsCloud.Settings.SettingsForStorage
{
    public interface IStorageSettings
    {
        string PathToCustomText { get; }
        string PathToSave { get; }

        ImageFormat ImageFormat { get; }
    }
}