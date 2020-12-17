using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TagsCloud.Infrastructure;

namespace TagsCloud.UiActions
{
    public class RenderFileAction : IUiAction
    {
        private readonly IImageHolder holder;
        public RenderFileAction(IImageHolder holder)
        {
            this.holder = holder;
        }

        public string Category => "Файл";
        public string Name => "Визуализировать файл...";
        public string Description => "Выбрать файл для визуализации облака тегов";

        public void Perform()
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                InitialDirectory = Path.GetFullPath(Assembly.GetExecutingAssembly().Location),
                DefaultExt = "txt",
                Filter = "Текстовые файлы (*.txt)|*.txt|"
                         +"Документы (*.doc;*.docx)|*.doc;*.docx"
            };

            var res = dialog.ShowDialog();

            if (res != DialogResult.OK) 
                return;

            holder.RenderWordsFromFile(dialog.FileName)
                .OnFail(error => MessageBox.Show(error, "Не удалось построить облако"));
        }
    }
}