using TagsCloudContainer.Common;
using TagsCloudContainer.UiActions;

namespace TagsCloudContainer.Actions
{
    internal class GradientSettingsAction : IUiAction
    {
        private readonly ColorSettingsProvider colorSettingsProvider;
        private readonly Gradient gradient;

        public GradientSettingsAction(Gradient gradient, ColorSettingsProvider colorSettingsProvider)
        {
            this.gradient = gradient;
            this.colorSettingsProvider = colorSettingsProvider;
        }

        public string Category => "Цвет";
        public string Name => "Градиент...";
        public string Description => "Использовать градиент для рисования облака тегов";

        public void Perform()
        {
            colorSettingsProvider.ColorSettings = gradient;
            SettingsForm.For(gradient).ShowDialog();
        }
    }
}