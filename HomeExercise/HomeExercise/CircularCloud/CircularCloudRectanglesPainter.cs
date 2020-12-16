using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using HomeExercise.Settings;


namespace HomeExercise
{
    public class CircularCloudRectanglesPainter: IPainter
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private Pen pen;
        private Color color;
        private readonly Random randomizer;
        private readonly List<Rectangle> rectangles;
        private readonly string fileName;
        private readonly ImageFormat format;
        private readonly int offsetX;
        private readonly int offsetY;

        public CircularCloudRectanglesPainter(List<Rectangle> rectangles,PainterSettings settings)
        {
            offsetX = settings.Size.Width/2;
            offsetY = settings.Size.Height/2;
            format = settings.Format;
            fileName = settings.FileName;
            this.rectangles = rectangles;
            randomizer= new Random();
            color = new Color();
            bitmap = new Bitmap(settings.Size.Width, settings.Size.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DrawFigures()
        {
            var center = rectangles.First();
            foreach (var rectangle in rectangles)
            {
                var newX= rectangle.X + offsetX - center.X;
                var newY = rectangle.Y + offsetY - center.Y;
                var point = new Point(newX, newY);
                var newRectangle = new Rectangle(point, rectangle.Size);

                color = GetColor();
                pen = new Pen(color);
                graphics.DrawRectangle(pen, newRectangle);
            }
            
            bitmap.Save(fileName, format);
        }

        private Color GetColor()
        {
            var R = randomizer.Next(0, 255);
            var G = randomizer.Next(0, 255);
            var B = randomizer.Next(0, 255);
            
            return Color.FromArgb(R, G, B);
        }
    }
}