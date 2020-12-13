namespace TagsCloud.Options
{
    public interface IProgramOptions
    {
        public IImageOptions ImageOptions { get; }

        public IFontOptions FontOptions { get; }

        public IFilterOptions FilterOptions { get; }

        public string FilePath { get; }
    }
}