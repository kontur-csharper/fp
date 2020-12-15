using System.Collections.Immutable;
using TagsCloudContainer.Infrastructure.Settings;
using YandexMystem.Wrapper.Enums;

namespace TagsCloudContainer.App.Settings
{
    public class FilteringWordsSettings : IFilteringWordsSettingsHolder
    {
        public static readonly ImmutableHashSet<GramPartsEnum> DefaultBoringGramParts = ImmutableHashSet.Create(
            GramPartsEnum.Interjection,
            GramPartsEnum.NounPronoun,
            GramPartsEnum.PronounAdjective,
            GramPartsEnum.Conjunction,
            GramPartsEnum.Pretext);

        public static readonly FilteringWordsSettings Instance = new FilteringWordsSettings();

        private FilteringWordsSettings()
        {
            SetDefault();
        }

        public ImmutableHashSet<GramPartsEnum> BoringGramParts { get; set; }

        public void SetDefault()
        {
            BoringGramParts = DefaultBoringGramParts;
        }
    }
}