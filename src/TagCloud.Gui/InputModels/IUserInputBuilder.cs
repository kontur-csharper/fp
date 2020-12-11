using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloud.Gui.Localization;

namespace TagCloud.Gui.InputModels
{
    public interface IUserInputBuilder
    {
        UserInputField Field(UiLabel key);
        UserInputSizeField Size(UiLabel key, bool showAsPoint = false);
        UserInputColor Color(Color defaultValue, UiLabel key);
        UserInputColorPalette ColorPalette(UiLabel key, params Color[] initialValue);

        UserInputMultipleOptionsChoice<TService> MultipleChoice<TService>(
            IDictionary<string, TService> source,
            UiLabel key);

        UserInputOneOptionChoice<TService> SingleChoice<TService>(
            IDictionary<string, TService> source,
            UiLabel key);

        UserInputOneOptionChoice<TService> ServiceChoice<TService>(
            IEnumerable<TService> source, UiLabel key);
        
        UserInputMultipleOptionsChoice<TService> SeveralServicesChoice<TService>(
            IEnumerable<TService> source, UiLabel key);

        UserInputOneOptionChoice<TEnum> EnumChoice<TEnum>(UiLabel key) where TEnum : struct, Enum;
        UserInputMultipleOptionsChoice<TEnum> SeveralEnumValuesChoice<TEnum>(UiLabel key) where TEnum : struct, Enum;
    }
}