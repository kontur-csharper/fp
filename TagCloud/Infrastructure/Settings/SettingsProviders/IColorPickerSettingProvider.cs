using System.Collections.Generic;
using System.Drawing;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Settings.SettingsProviders
{
    public interface IColorPickerSettingProvider
    {
        public Dictionary<WordType, Color> ColorMap { get; }
    }
}