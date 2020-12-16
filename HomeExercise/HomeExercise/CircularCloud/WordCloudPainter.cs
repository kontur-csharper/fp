using System;
using System.Drawing;
using HomeExercise.Settings;
using ResultOf;

namespace HomeExercise
{
    public class WordCloudPainter : IPainter
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private readonly int offsetX;
        private readonly int offsetY;
        
        private readonly PainterSettings settings;
        private readonly IWordCloud wordCloud;

        public WordCloudPainter(IWordCloud wordCloud, PainterSettings settings)
        {
            wordCloud.BuildCloud();
            this.wordCloud = wordCloud;
            this.settings = settings;

            offsetX = settings.Size.Width/2;
            offsetY = settings.Size.Height/2;

            bitmap = new Bitmap(settings.Size.Width, settings.Size.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DrawFigures()
        {
            Result.Ok(this)
                .Then(p => DrawWords())
                .Then(p => SaveImage())
                .Then(p => CheckSize())
                .OnFail(Console.WriteLine); 
        }

        private void SaveImage()
        {
            var fileName = $"{settings.FileName}.{settings.Format}";
            bitmap.Save(fileName, settings.Format);
        }
        
        private void DrawWords()
        {
            foreach (var word in wordCloud.SizedWords)
            {
                DrawWord(word, wordCloud.Center);
            }
        }
        
        private void DrawWord(ISizedWord word, Point center)
        {
            var newX= word.Rectangle.X + offsetX - center.X;
            var newY = word.Rectangle.Y + offsetY - center.Y;
            var point = new Point(newX, newY);
            var newRectangle = new Rectangle(point, word.Rectangle.Size);
            var wordFont = new Font(word.Font, word.Size, FontStyle.Bold, GraphicsUnit.Point);
                
            var brush = new SolidBrush(settings.Color);
                
            graphics.DrawString(word.Text, wordFont, brush, newRectangle.Location);
        }

        private Result<None> CheckSize()
        {
            return settings.Size.Height < wordCloud.Size.Height || settings.Size.Width < wordCloud.Size.Width
                ? Result.Fail<None>("Image size is smaller than cloud size")
                : Result.Ok();
        }
    }
}