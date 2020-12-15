using TagsCloudContainer.Common;
using TagsCloudContainer.UiActions;

namespace TagsCloudContainer.Actions
{
    internal class RandomColorsSettingsAction : IUiAction
    {
        private readonly ColorSettingsProvider colorSettingsProvider;
        private readonly RandomColors randomColors;

        public RandomColorsSettingsAction(RandomColors randomColors, ColorSettingsProvider colorSettingsProvider)
        {
            this.randomColors = randomColors;
            this.colorSettingsProvider = colorSettingsProvider;
        }

        public string Category => "Цвет";
        public string Name => "Случайный выбор цветов...";
        public string Description => "Использовать случайные цвета для рисования облака тегов";

        public void Perform()
        {
            colorSettingsProvider.ColorSettings = randomColors;
            SettingsForm.For(randomColors).ShowDialog();
        }
    }
}