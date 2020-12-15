namespace TagsCloudContainer.Common
{
    public class ImageSettings : ISettings
    {
        private static readonly int defaultWidth = 900;
        private static readonly int defaultHeight = 700;
        public int Width { get; set; } = defaultWidth;
        public int Height { get; set; } = defaultHeight;

        public Result<ISettings> CheckSettings()
        {
            if (Width <= 0)
            {
                Width = defaultWidth;
                return new Result<ISettings>("Размеры изображения должны быть положительными");
            }

            if (Height <= 0)
            {
                Height = defaultHeight;
                return new Result<ISettings>("Размеры изображения должны быть положительными");
            }

            if (Width > 900)
            {
                Width = defaultWidth;
                return new Result<ISettings>("Максимальные размеры изображения 900*700");
            }

            if (Height > 700)
            {
                Height = defaultHeight;
                return new Result<ISettings>("Максимальные размеры изображения 900*700");
            }

            return new Result<ISettings>(null, this);
        }
    }
}