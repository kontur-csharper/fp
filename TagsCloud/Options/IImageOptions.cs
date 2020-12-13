namespace TagsCloud.Options
{
    public interface IImageOptions
    {
        public int Width { get; }

        public int Height { get; }

        public string ImageOutputDirectory { get; }

        public string ImageName { get; }

        public string ImageExtension { get; }
    }
}