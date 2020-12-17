using System.Windows.Forms;
using TagsCloud.Infrastructure;
using TagsCloud.Layouters;

namespace TagsCloud.UiActions
{
    public class SelectDenseLayouterAction : IUiAction
    {
        private IImageHolder holder;
        private CircularCloudLayouter newLayouter;
        public SelectDenseLayouterAction(IImageHolder holder, CircularCloudLayouter layouter)
        {
            this.holder = holder;
            newLayouter = layouter;
        }

        public string Category => "Алгоритм построения облака";
        public string Name => "Плотное построение";
        public string Description => "Размещает слова плотно, основываясь на границах их размеров";

        public void Perform()
        {
            holder.ChangeLayouter(newLayouter)
                .OnFail(error => MessageBox.Show(error, "Не удалось корректно перестроить облако"));
        }
    }
}
