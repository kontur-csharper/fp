using System;
using System.Collections.Generic;
using System.Drawing;
using Autofac;
using CommandLine;
using TagsCloud.Drawer;
using TagsCloud.Layouter;
using TagsCloud.Options;
using TagsCloud.ResultOf;
using TagsCloud.WordsParser;

namespace TagsCloud
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ITagCloud cloud = null;

            MakeProgramOptions(args)
                .Then(BuildProgram)
                .Then(container => cloud = container.Resolve<ITagCloud>())
                .Then(_ => cloud.MakeTagCloud())
                .Then(_ => cloud.SaveTagCloud())
                .OnFail(Console.WriteLine);
        }

        private static IContainer BuildProgram(IProgramOptions programOptions)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(programOptions.ImageOptions).As<IImageOptions>();
            builder.RegisterInstance(programOptions.FontOptions).As<IFontOptions>();
            builder.RegisterInstance(programOptions.FilterOptions).As<IFilterOptions>();
            builder.RegisterType<WordsAnalyzer>().As<IWordsAnalyzer>();
            builder.RegisterType<FileReader>().As<IWordReader>().WithParameter("filePath", programOptions.FilePath);
            builder.RegisterType<Filter>().As<IFilter>();
            builder.RegisterType<CircularCloudLayouter>().As<ILayouter>()
                .WithParameter("center",
                    new Point(programOptions.ImageOptions.Width / 2, programOptions.ImageOptions.Height / 2));
            builder.RegisterType<LayoutDrawer>().As<ILayoutDrawer>();
            builder.RegisterType<RectangleLayout>().As<IRectangleLayout>();
            builder.RegisterType<TagCloud>().As<ITagCloud>();
            return builder.Build();
        }

        private static Result<ProgramOptions> MakeProgramOptions(IEnumerable<string> args)
        {
            var options = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
            if (options == null)
                Result.Fail<ProgramOptions>("");
            var imageOptions = OptionsValidator.ValidateImageOptions(options);
            var fontOptions = OptionsValidator.ValidateFontOptions(options);
            var filterOptions = OptionsValidator.ValidateFilterOptions(options);

            return options is null
                ? Result.Fail<ProgramOptions>("")
                : imageOptions
                    .Then(_ => fontOptions)
                    .Then(_ => filterOptions)
                    .Then(_ => new ProgramOptions(imageOptions.Value, fontOptions.Value, filterOptions.Value,
                        options.FilePath));
        }
    }
}