using System;

namespace TagsCloud.Infrastructure
{
    public class ImageSize
    {
        private int width, height;

        public ImageSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width
        {
            get => width;
            set
            {
                if (value <= 0)
                    throw new ArgumentException($"Value must be greater than 0, but found {value}");
                width = value;
            }
        }

        public int Height
        {
            get => height;
            set
            {
                if (value <= 0)
                    throw new ArgumentException($"Value must be greater than 0, but found {value}");
                height = value;
            }
        }

        public static ImageSize operator *(ImageSize size, double factor)
        {
            return new ImageSize((int) (size.Width * factor), (int) (size.Height * factor));
        }
    }
}