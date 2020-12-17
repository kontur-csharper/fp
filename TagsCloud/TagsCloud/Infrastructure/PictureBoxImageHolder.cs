using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TagsCloud.Layouters;
using TagsCloud.WordsProcessing;

namespace TagsCloud.Infrastructure
{
    public class PictureBoxImageHolder : PictureBox, IImageHolder
    {
        public ImageSettings Settings { get; }
        public string SettingsErrorMessage { get; private set; }
        private ICloudLayouter layouter;
        private Dictionary<string, int> wordsFreuqencies;
        private IWordsFrequencyParser parser;
        private string previousFileName;

        public PictureBoxImageHolder(IWordsFrequencyParser parser, ImageSettings settings, ICloudLayouter layouter)
        {
            Settings = settings;
            this.layouter = layouter;
            this.parser = parser;
            RecreateCanvas(settings);
            this.Size = new Size(settings.Width, settings.Height);

            ImageSettings.SettingsIsChanged += (sender, args) =>
            {
                RedrawCurrentImage()
                    .OnFail(_ => OnError());
                Size = new Size(settings.Width, settings.Height);
            };
        }

        void IImageHolder.OnError() => OnError();

        public static event EventHandler Error;

        event EventHandler IImageHolder.Error
        {
            add => Error += value;
            remove => Error -= value;
        }

        public Result<None> RenderWordsFromFile(string fileName)
        {
            return ProcessFile(fileName)
                .Then(_ => layouter.ClearLayouter())
                .Then(_ => PlaceWords())
                .OnFail(error => SettingsErrorMessage = error);
        }

        public static void OnError() =>
            Error.Invoke(null, EventArgs.Empty);

        public Result<None> ChangeLayouter(ICloudLayouter layouter)
        {
            this.layouter = layouter;
            return RedrawCurrentImage();
        }

        public Result<None> RedrawCurrentImage()
        {
            RecreateCanvas(Settings);
            return RenderWordsFromFile(previousFileName);
        }

        public Result<None> SaveImage(string fileName)
        {
            return CheckFormat(fileName)
                .Then(format => Image.Save(fileName, format));
        }

        public void UpdateUi()
        {
            Refresh();
            Application.DoEvents();
        }

        public void RecreateCanvas(ImageSettings imageSettings)
        {
            Image = new Bitmap(imageSettings.Width, imageSettings.Height, PixelFormat.Format24bppRgb);
            layouter.UpdateCenterPoint(imageSettings);
            DrawBaseCanvas();
        }

        private Result<ImageFormat> CheckFormat(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension switch
            {
                ".jpeg" => Result.Ok(ImageFormat.Jpeg),
                ".png" => Result.Ok(ImageFormat.Png),
                ".tiff" => Result.Ok(ImageFormat.Tiff),
                _ => Result.Fail<ImageFormat>("Неподдерживаемое расширение файла")
            };
        }


        private Result<None> ProcessFile(string fileName)
        {
            RecreateCanvas(Settings);
            if (string.IsNullOrEmpty(previousFileName) && !string.IsNullOrEmpty(fileName) || previousFileName != fileName)
            {
                if (previousFileName != fileName)
                    layouter.ClearLayouter();

                if (!File.Exists(fileName))
                    return Result.Fail<None>("Запрошенный файл не найден");

                return parser.ParseWordsFrequencyFromFile(fileName)
                    .Then(freq =>
                    {
                        wordsFreuqencies = freq;
                        previousFileName = fileName;
                    });
            }

            return Result.Ok();
        }

        private Result<None> PlaceWords()
        {
            if (wordsFreuqencies is null)
                return Result.Ok();

            var totalWords = wordsFreuqencies.Sum(x => x.Value);
            var graphics = StartDrawing();

            foreach (var pair in wordsFreuqencies.OrderByDescending(x => x.Value))
            {
                var fontSize = FontSizeProvider.GetFontSize(Settings.Font.Size, 100 / (double)totalWords * pair.Value / 100);

                var label = new Label { AutoSize = true };
                label.Font = new Font(Settings.Font.FontFamily, fontSize, Settings.Font.Style);
                label.Text = pair.Key;
                var size = label.GetPreferredSize(label.GetPreferredSize(Size));

                var placingResult = layouter.PutNextRectangle(size);
                if (!placingResult.IsSuccess)
                    return Result.Fail<None>(placingResult.Error);
                var rect = placingResult.Value;

                graphics.DrawString(pair.Key, label.Font, new SolidBrush(Settings.Palette.TextColor), rect);
                UpdateUi();
            }

            return Result.Ok();
        }

        private Graphics StartDrawing() => Graphics.FromImage(Image);

        private void DrawBaseCanvas()
        {
            var g = StartDrawing();
            g.FillRectangle(new SolidBrush(Settings.Palette.BackgroundColor),
                new Rectangle(0, 0, Settings.Width, Settings.Height));
            UpdateUi();
        }
    }
}