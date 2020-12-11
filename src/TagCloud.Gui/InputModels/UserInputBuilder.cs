using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Gui.Localization;

namespace TagCloud.Gui.InputModels
{
    public class UserInputBuilder : IUserInputBuilder
    {
        private readonly ILocalizationProvider provider;

        public UserInputBuilder(ILocalizationProvider provider)
        {
            this.provider = provider;
        }

        public UserInputField Field(UiLabel key) => new UserInputField(provider.GetLabel(key));

        public UserInputSizeField Size(UiLabel key, bool showAsPoint = false) =>
            new UserInputSizeField(provider.GetLabel(key),
                provider.GetLabel(showAsPoint ? UiLabel.XPoint : UiLabel.Width),
                provider.GetLabel(showAsPoint ? UiLabel.YPoint : UiLabel.Height));

        public UserInputMultipleOptionsChoice<TService> MultipleChoice<TService>(
            IDictionary<string, TService> source,
            UiLabel key) =>
            new UserInputMultipleOptionsChoice<TService>(provider.GetLabel(key), SelectorItems(source));

        public UserInputOneOptionChoice<TService> SingleChoice<TService>(
            IDictionary<string, TService> source,
            UiLabel key) =>
            new UserInputOneOptionChoice<TService>(provider.GetLabel(key), SelectorItems(source));

        public UserInputColor Color(Color defaultValue, UiLabel key) =>
            new UserInputColor(provider.GetLabel(key), defaultValue);

        public UserInputColorPalette ColorPalette(UiLabel key, params Color[] initialValue) =>
            new UserInputColorPalette(provider.GetLabel(key), initialValue,
                provider.GetLabel(UiLabel.ButtonAdd),
                provider.GetLabel(UiLabel.ButtonRemove));

        public UserInputOneOptionChoice<TService> ServiceChoice<TService>(IEnumerable<TService> source, UiLabel key) =>
            SingleChoice(ToDictionaryByName(source), key);

        public UserInputMultipleOptionsChoice<TService> SeveralServicesChoice<TService>(IEnumerable<TService> source,
            UiLabel key) =>
            MultipleChoice(ToDictionaryByName(source), key);

        public UserInputOneOptionChoice<TEnum> EnumChoice<TEnum>(UiLabel key) where TEnum : struct, Enum =>
            SingleChoice(DictionaryFromEnum<TEnum>(), key);

        public UserInputMultipleOptionsChoice<TEnum> SeveralEnumValuesChoice<TEnum>(UiLabel key)
            where TEnum : struct, Enum =>
            MultipleChoice(DictionaryFromEnum<TEnum>(), key);

        private Dictionary<string, TEnum> DictionaryFromEnum<TEnum>() where TEnum : struct, Enum =>
            Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToDictionary(provider.Get);

        private Dictionary<string, TService> ToDictionaryByName<TService>(IEnumerable<TService> source) =>
            source.Where(x => x != null).ToDictionary(x => provider.Get(x.GetType()));

        private UserInputSelectorItem<T>[] SelectorItems<T>(IDictionary<string, T> source) =>
            source.Select(x => new UserInputSelectorItem<T>(x.Key, x.Value))
                .OrderBy(x => x.Name)
                .ToArray();
    }
}