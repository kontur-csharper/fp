namespace TagsCloud.Options
{
    public class ProgramOptions : IProgramOptions
    {
        public IImageOptions ImageOptions { get; }
        public IFontOptions FontOptions { get; }
        public IFilterOptions FilterOptions { get; }
        public string FilePath { get; }

        public ProgramOptions(IImageOptions imageOptions, IFontOptions fontOptions, IFilterOptions filterOptions,
            string filePath)
        {
            ImageOptions = imageOptions;
            FontOptions = fontOptions;
            FilterOptions = filterOptions;
            FilePath = filePath;
        }
    }
}