using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.Settings
{
    public class OutputSettings : IOutputSettingsHolder
    {
        public static readonly string DefaultOutputDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "image.png");

        public static readonly OutputSettings Instance = new OutputSettings();

        private OutputSettings()
        {
            SetDefault();
        }

        public string OutputFilePath { get; set; }

        public ImageFormat ImageFormat => ParseImageFormat(Path.GetExtension(OutputFilePath).Substring(1));

        public void SetDefault()
        {
            OutputFilePath = DefaultOutputDirectory;
        }

        private static ImageFormat ParseImageFormat(string str)
        {
            return (ImageFormat) typeof(ImageFormat)
                .GetProperty(str, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                .GetValue(null);
        }
    }
}