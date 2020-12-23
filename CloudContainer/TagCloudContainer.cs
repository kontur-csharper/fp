using System.Drawing;
using System.IO;
using TagsCloudVisualization;
using TagsCloudVisualization.Configs;
using TagsCloudVisualization.WordsCleaners;
using TagsCloudVisualization.WordsConverters;
using TagsCloudVisualization.WordsProviders;

namespace CloudContainer
{
    public class TagCloudContainer
    {
        private readonly IWordsCleaner cleaner;
        private readonly IConfig config;
        private readonly IWordConverter converter;
        private readonly IWordProvider provider;


        public TagCloudContainer(IWordsCleaner cleaner, IWordConverter converter,
            IWordProvider provider, IConfig config)
        {
            this.cleaner = cleaner;
            this.converter = converter;
            this.provider = provider;
            this.config = config;
        }


        public Result<Bitmap> GetImage(TagCloudArguments arguments)
        {
            config.SetValues(arguments.Font, arguments.Center,
                arguments.TextColor, arguments.ImageSize, arguments.BoringWords);
            cleaner.AddBoringWords(config.BoringWords);

            var path = arguments.InputFileName;
            return provider
                .GetWords(path)
                .Then(x => cleaner.CleanWords(x)
                    .Then(y => converter.ConvertWords(y)
                        .Then(z => Drawer.DrawImage(z, config))));
        }
    }
}