using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Infrastructure.Settings.SettingsProviders;
using TagCloud.Infrastructure.Settings.UISettingsManagers.Interfaces;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public class FontSettingManager : IOptionsManager
    {
        private readonly Func<IFontSettingProvider> settingProvider;

        public FontSettingManager(Func<IFontSettingProvider> settingProvider)
        {
            this.settingProvider = settingProvider;
        }

        public string Title => "Font";
        public string Help => "Choose font family name to write tags";

        public Result<string> TrySet(string name)
        {
            var fontFamily = new FontFamily(name);
            if (fontFamily.Name == name)
                settingProvider().FontFamily = fontFamily;
            else
                return Result.Fail<string>($"FontFamily {name} was not found!");
            return Get();
        }

        public string Get()
        {
            return settingProvider().FontFamily.Name;
        }

        public IEnumerable<string> GetOptions()
        {
            return FontFamily.Families.Select(font => font.Name);
        }

        public string GetSelectedOption()
        {
            return settingProvider().FontFamily.Name;
        }
    }
}