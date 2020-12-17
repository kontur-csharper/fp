using TagsCloud.GUI;
using TagsCloud.Infrastructure;

namespace TagsCloud.UiActions
{
    public class ImageSettingsAction : IUiAction
    {
        private readonly IImageHolder holder;
        public ImageSettingsAction(IImageHolder holder)
        {
            this.holder = holder;
        }

        public string Category => "Настройки";
        public string Name => "Изображение...";
        public string Description => "Размеры изображения";

        public void Perform()
        {
            new SettingsForm(holder).ShowDialog();
        }
    }
}