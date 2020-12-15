using TagsCloudContainer.Common;
using TagsCloudContainer.UiActions;

namespace TagsCloudContainer.Actions
{
    public class PaletteSettingsAction : IUiAction
    {
        private readonly ColorSettingsProvider colorSettingsProvider;
        private readonly Palette palette;

        public PaletteSettingsAction(Palette palette, ColorSettingsProvider colorSettingsProvider)
        {
            this.palette = palette;
            this.colorSettingsProvider = colorSettingsProvider;
        }

        public string Category => "Цвет";
        public string Name => "Палитра...";
        public string Description => "Использовать палитру для рисования облака тегов";

        public void Perform()
        {
            colorSettingsProvider.ColorSettings = palette;
            SettingsForm.For(palette).ShowDialog();
        }
    }
}