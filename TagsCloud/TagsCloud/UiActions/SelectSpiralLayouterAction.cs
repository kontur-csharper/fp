using System.Windows.Forms;
using TagsCloud.Infrastructure;
using TagsCloud.Layouters;

namespace TagsCloud.UiActions
{
    public class SelectSpiralLayouterAction : IUiAction
    {
        private readonly IImageHolder holder;
        private readonly SpiralCloudLayouter newLayouter;
        public SelectSpiralLayouterAction(IImageHolder holder, SpiralCloudLayouter layouter)
        {
            this.holder = holder;
            newLayouter = layouter;
        }

        public string Category => "Алгоритм построения облака";
        public string Name => "Спиральное построение";
        public string Description => "Размещает слова по спирали";

        public void Perform()
        {
            holder.ChangeLayouter(newLayouter)
                .OnFail(error => MessageBox.Show(error, "Не удалось корректно перестроить облако"));
        }
    }
}
