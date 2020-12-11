using System;
using System.Globalization;

namespace TagCloud.Gui.Localization
{
    public interface ILocalizationSource
    {
        CultureInfo ForCulture { get; }

        bool TryGet(Type type, out string value);
        bool TryGet<T>(T enumItem, out string value) where T : struct, Enum;
        bool TryGetLabel(UiLabel key, out string value);
    }
}