using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TagCloud.Gui.Localization
{
    public class LocalizationProvider : ILocalizationProvider
    {
        private readonly ILocalizationSource? defaultSource;
        private readonly ILocalizationSource? localizationSource;

        public LocalizationProvider(IEnumerable<ILocalizationSource> sources)
        {
            var dictionary = sources.ToDictionary(x => x.ForCulture);

            localizationSource = GetMostSpecificLocalizationOrNull(CultureInfo.CurrentUICulture, dictionary);
            defaultSource = dictionary.GetValueOrDefault(CultureInfo.GetCultureInfo("en"));
        }

        public string Get(Type type) =>
            localizationSource != null && localizationSource.TryGet(type, out var result) ||
            defaultSource != null && defaultSource.TryGet(type, out result)
                ? result
                : type.Name;

        public string Get<T>(T enumItem) where T : struct, Enum =>
            localizationSource != null && localizationSource.TryGet(enumItem, out var result) ||
            defaultSource != null && defaultSource.TryGet(enumItem, out result)
                ? result
                : enumItem.ToString();

        public string GetLabel(UiLabel key) =>
            localizationSource != null && localizationSource.TryGetLabel(key, out var result) ||
            defaultSource != null && defaultSource.TryGetLabel(key, out result)
                ? result
                : key.ToString();

        // to make possible resolve source when current culture = en-US is missing, but en localization is specified
        private static ILocalizationSource? GetMostSpecificLocalizationOrNull(CultureInfo cultureInfo,
            IDictionary<CultureInfo, ILocalizationSource> sources)
        {
            while (!Equals(cultureInfo, CultureInfo.InvariantCulture))
            {
                if (sources.TryGetValue(cultureInfo, out var source))
                    return source;
                cultureInfo = cultureInfo.Parent;
            }

            return null;
        }
    }
}