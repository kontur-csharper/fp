using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Infrastructure.CloudVisualizer;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App
{
    internal class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        private readonly IImageSizeSettingsHolder sizeSettings;
        private readonly IOutputSettingsHolder outputSettings;
        private readonly Lazy<MainForm> mainForm;
        private readonly ICloudVisualizer cloudVisualizer;

        public PictureBoxImageHolder(IImageSizeSettingsHolder sizeSettings,
            IOutputSettingsHolder outputSettings, Lazy<MainForm> mainForm,
            ICloudVisualizer cloudVisualizer)
        {
            this.sizeSettings = sizeSettings;
            this.outputSettings = outputSettings;
            this.mainForm = mainForm;
            this.cloudVisualizer = cloudVisualizer;
            RecreateImage();
        }

        public void GenerateImage()
        {
            mainForm.Value.SetEnabled(false);
            var thread = new Thread(Visualize);
            thread.Start();
        }

        public Graphics StartDrawing()
        {
            return Graphics.FromImage(Image);
        }

        public void UpdateUi()
        {
            MethodInvoker method = delegate
            {
                mainForm.Value.SetEnabled(true);
                Refresh();
                Application.DoEvents();
            };
            if (mainForm.Value.InvokeRequired)
                BeginInvoke(method);
            else
                method.Invoke();
        }

        public void RecreateImage()
        {
            if (Image != null)
                mainForm.Value.ClientSize = new Size(sizeSettings.Width, sizeSettings.Height);
            Image = new Bitmap(sizeSettings.Width,
                sizeSettings.Height, PixelFormat.Format24bppRgb);
        }

        public void SaveImage()
        {
            Image.Save(outputSettings.OutputFilePath, outputSettings.ImageFormat);
        }

        private void Visualize()
        {
            var result = cloudVisualizer.Visualize();
            if (!result.IsSuccess)
                MessageBox.Show(result.Error);
            UpdateUi();
        }
    }
}