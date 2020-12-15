using System.IO;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.Settings
{
    public class InputSettings : IInputSettingsHolder
    {
        public static readonly string DefaultInputFileName = Path.Combine(
            Directory.GetCurrentDirectory(),
            "text.txt");

        public static readonly InputSettings Instance = new InputSettings();

        private InputSettings()
        {
            SetDefault();
        }

        public string InputFileName { get; set; }

        public void SetDefault()
        {
            InputFileName = DefaultInputFileName;
        }
    }
}