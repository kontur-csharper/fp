using System.Windows.Forms;

namespace TagsCloudVisualization.Infrastructure.Common
{
    public static class InformationMessageHelper
    {
        public static void ShowExceptionMessage(string inform) =>
            MessageBox.Show(
                inform,
                "Message",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly
            );
    }
}