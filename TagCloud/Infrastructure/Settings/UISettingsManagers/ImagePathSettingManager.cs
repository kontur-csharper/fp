using System;
using System.IO;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class ImagePathSettingManager : IInputManager
    {
        private readonly Func<IImageFormatSettingProvider> imageFormatSettingsProvider;
        private readonly Func<IImageSettingsProvider> imageSettingsProvider;

        public ImagePathSettingManager(Func<IImageSettingsProvider> imageSettingsProvider,
            Func<IImageFormatSettingProvider> imageFormatSettingsProvider)
        {
            this.imageSettingsProvider = imageSettingsProvider;
            this.imageFormatSettingsProvider = imageFormatSettingsProvider;
        }

        public string Title => "Image path";
        public string Help => "Type image file location to save";

        public Result<string> TrySet(string path)
        {
            path = Path.GetFullPath(path);
            if (Directory.Exists(path))
                return Result.Fail<string>("Path cannot be a directory");
            imageSettingsProvider().ImagePath = path;
            if (Path.HasExtension(path))
                return Result.Fail<string>(
                    $"Specify filename only .{imageFormatSettingsProvider().Format} will be added");
            var convertedPath = Path.ChangeExtension(path, imageFormatSettingsProvider().Format.ToString());
            if (File.Exists(convertedPath))
                return Result.Fail<string>($"File {convertedPath} already exists and will be overwritten!");
            return Get();
        }

        public string Get()
        {
            return imageSettingsProvider().ImagePath;
        }
    }
}