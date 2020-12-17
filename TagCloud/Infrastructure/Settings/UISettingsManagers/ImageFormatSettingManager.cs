using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class ImageFormatSettingManager : IOptionsManager
    {
        private readonly Func<IImageFormatSettingProvider> settingsProvider;

        public ImageFormatSettingManager(Func<IImageFormatSettingProvider> settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public IEnumerable<string> GetOptions()
        {
            return typeof(ImageFormat)
                .GetProperties()
                .Select(info => info.Name);
        }

        public string GetSelectedOption()
        {
            return settingsProvider().Format.ToString();
        }

        public string Title => "Image Format";
        public string Help => "Choose format";

        public Result<string> TrySet(string extension)
        {
            var propertyInfos = typeof(ImageFormat)
                .GetProperties();

            var newFormat = propertyInfos
                .Where(info => info.Name.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                .Select(info => info.GetValue(settingsProvider().Format))
                .Where(f => f is ImageFormat)
                .Cast<ImageFormat>()
                .SingleOrDefault();

            if (newFormat == null)
                return Result.Fail<string>($"{extension} is not supported extension");
            settingsProvider().Format = newFormat;
            return Get();
        }

        public string Get()
        {
            return settingsProvider().Format.ToString();
        }
    }
}