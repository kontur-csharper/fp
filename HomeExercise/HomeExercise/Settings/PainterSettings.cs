using System.Drawing;
using System.Drawing.Imaging;

namespace HomeExercise.Settings
{
    public class PainterSettings
    {
        public Color Color { get; }
        public Size Size { get; }
        public string FileName { get; }
        public ImageFormat Format { get; }
        
        public PainterSettings(Size size, string fileName, ImageFormat format, Color color)
        {
            Size = size;
            FileName = fileName;
            Format = format;
            Color = color;
        }
    }
}