using System;

namespace TagCloud.Gui.Localization
{
    public interface ILocalizationProvider
    {
        string Get(Type type);
        string Get<T>(T enumItem) where T : struct, Enum;
        string GetLabel(UiLabel key);
    }
}