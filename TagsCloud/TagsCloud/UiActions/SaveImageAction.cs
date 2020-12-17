using System.Windows.Forms;
using TagsCloud.Infrastructure;

namespace TagsCloud.UiActions
{
    public class SaveImageAction : IUiAction
    {
        private readonly IImageHolder imageHolder;
        public SaveImageAction(IImageHolder holder)
        {
            imageHolder = holder;
        }

        public string Category => "Файл";
        public string Name => "Сохранить как...";
        public string Description => "Сохранить изображение в файл";

        public void Perform()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить как...",
                CheckFileExists = false,
                AddExtension = true,
                FileName = "image",
                Filter = "Изображения (*.png)|*.png|" + 
                         "Jpeg (*.jpeg)|*.jpeg|" +
                         "TIFF (*.tiff)|*.tiff"
            };

            var res = dialog.ShowDialog();

            if (res == DialogResult.OK)
                imageHolder.SaveImage(dialog.FileName)
                    .OnFail(error => MessageBox.Show(error, "Не удалось сохранить изображение"));
        }
    }
}