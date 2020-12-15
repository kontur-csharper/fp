using System.Windows.Forms;
using TagsCloudContainer.Common;
using TagsCloudContainer.UiActions;

namespace TagsCloudContainer.Actions
{
    public class SaveImageAction : IUiAction
    {
        private readonly IImageHolder imageHolder;

        public SaveImageAction(IImageHolder imageHolder)
        {
            this.imageHolder = imageHolder;
        }

        public string Category => "Файлы";
        public string Name => "Сохранить облако тегов...";
        public string Description => "Сохранить изображение в файл";

        public void Perform()
        {
            var dialog = new SaveFileDialog
            {
                CheckFileExists = false,
                DefaultExt = "png",
                FileName = "picture.png",
                Filter = "Image Files(*.BMP;*.JPEG;*.JPG;*.PNG;)|*.BMP;*.JPEG;*.JPG;*.PNG|All files (*.*)|*.*"
            };
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK) imageHolder.SaveImage(dialog.FileName);
        }
    }
}