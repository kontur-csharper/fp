using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TagsCloudContainer.Common;

namespace TagsCloudContainer
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public Result<Size> GetImageSize()
        {
            if (!FailIfNotInitialized().IsSuccess)
                return new Result<Size>("ImageHolder не инициализирован");
            return new Result<Size>(null, Image.Size);
        }

        public Result<Graphics> StartDrawing()
        {
            if (!FailIfNotInitialized().IsSuccess)
                return new Result<Graphics>("ImageHolder не инициализирован");
            return new Result<Graphics>(null, Graphics.FromImage(Image));
        }

        public void UpdateUi()
        {
            Refresh();
            Application.DoEvents();
        }

        public void RecreateImage(ImageSettings imageSettings)
        {
            Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
        }

        public Result<None> SaveImage(string fileName)
        {
            if (!FailIfNotInitialized().IsSuccess)
                return new Result<None>("ImageHolder не инициализирован");

            Image.Save(fileName);
            return new Result<None>(null);
        }

        private Result<None> FailIfNotInitialized()
        {
            if (Image == null)
                return new Result<None>(
                    "Call PictureBoxImageHolder.RecreateImage before other method call!");
            return new Result<None>(null);
        }
    }
}