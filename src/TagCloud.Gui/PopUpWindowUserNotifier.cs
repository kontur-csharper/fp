using System.Windows.Forms;
using TagCloud.Core.Text;

namespace TagCloud.Gui
{
    public class PopUpWindowUserNotifier : IUserNotifier
    {
        public void Notify(string message) => MessageBox.Show(
            message, 
            "Tag cloud layouter", 
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}