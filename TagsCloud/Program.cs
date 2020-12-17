using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using ResultOf;
using TagsCloud.BitmapCreator;
using TagsCloud.ColoringAlgorithms;
using TagsCloud.FileReaders;
using TagsCloud.ImageConfig;
using TagsCloud.LayoutAlgorithms;
using TagsCloud.PointGenerator;
using TagsCloud.WordFilters;


namespace TagsCloud
{
    public static class Program
    {
        private static string _pathToInputFile;
        private static Color _backgroundColor = Color.Bisque;
        private static Color _textColor = Color.Aquamarine;
        private static FontFamily _fontFamily = new FontFamily("Arial");
        private static string _format = "png";
        private static Size _size = new Size(1000, 1000);
        private static IServiceProvider _serviceProvider;

        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            ConfigureCommandLineApp(app);
            try
            {
                app.Execute(args);
            }
            catch (Exception)
            {
                ErrorHandler.ThrowError("Введенная вами команда некоректна. Для подробностей --help");
            }

            ConfigureServices();

            var textReader = _serviceProvider.GetService<IFileReader>();
            var wordFilter = _serviceProvider.GetService<IWordFilter>();
            var bitmapCreator = _serviceProvider.GetService<IBitmapCreator>();

            var filePath = textReader.GetWordsFromFile(_pathToInputFile)
                .Then(x => wordFilter.FilterWords(x))
                .Then(x => bitmapCreator.Create(x))
                .Then(x => BitmapSaver.BitmapSaver.Save(x, _format));

            if (!filePath.IsSuccess)
                ErrorHandler.ThrowError(filePath.Error);

            Console.WriteLine($"File was saved on {filePath.Value}");

            bitmapCreator.Dispose();
            return 0;
        }

        private static void ConfigureCommandLineApp(CommandLineApplication app)
        {
            var help = app.Option("-h|--help <HELP>", "Help", CommandOptionType.NoValue);
            var input = app.Option("-i|--input <INPUT>", "Input file path", CommandOptionType.SingleValue);
            var backgroundColor = app.Option("-b|--bgcolor <BGCOLOR>", "Background color",
                CommandOptionType.SingleValue);
            var textColor = app.Option("-t|--tagcolor <TAGCOLOR>", "Tags color", CommandOptionType.SingleValue);
            var font = app.Option("-f|--font <FONT>", "Font family", CommandOptionType.SingleValue);
            var format = app.Option("-r|--format <FORMAT>", "Result image format. Only png, jpg, jpeg, bmp",
                CommandOptionType.SingleValue);
            var size = app.Option<Size>("-s|--size <SIZE>", "Result image size", CommandOptionType.SingleValue);

            app.OnExecute(() =>
                {
                    if (help.HasValue())
                    {
                        foreach (var i in app.Options)
                            Console.WriteLine($"--{i.LongName}, -{i.ShortName} is {i.Description}");
                        Process.GetCurrentProcess().Kill();
                    }

                    if (input.HasValue())
                    {
                        var filePathResult = ArgumentParser.CheckFilePath(input.Value());
                        if (filePathResult.IsSuccess)
                            _pathToInputFile = filePathResult.Value;
                        else
                            ErrorHandler.ThrowError(filePathResult.Error);
                    }
                    else
                    {
                        ErrorHandler.ThrowError(
                            "ArgumentException. Аргумент -i: Не удалось обнаружить путь к файлу --help");
                    }

                    if (backgroundColor.HasValue())
                    {
                        var bgColorResult = ArgumentParser.GetColor(backgroundColor.Value());
                        if (bgColorResult.IsSuccess)
                            _backgroundColor = bgColorResult.Value;
                        else
                            ErrorHandler.ThrowError(bgColorResult.Error);
                    }

                    if (textColor.HasValue())
                    {
                        var textColorResult = ArgumentParser.GetColor(textColor.Value());
                        if (textColorResult.IsSuccess)
                            _textColor = textColorResult.Value;
                        else
                            ErrorHandler.ThrowError(textColorResult.Error);
                    }

                    if (font.HasValue())
                    {
                        var fontResult = ArgumentParser.GetFontFamily(font.Value());

                        if (fontResult.IsSuccess)
                            _fontFamily = fontResult.Value;
                        else
                            ErrorHandler.ThrowError(fontResult.Error);
                    }


                    if (format.HasValue())
                    {
                        var formatResult = ArgumentParser.IsCorrectFormat(format.Value());

                        if (formatResult.IsSuccess)
                            _format = format.Value();
                        else
                            ErrorHandler.ThrowError(formatResult.Error);
                    }

                    if (size.HasValue())
                    {
                        var sizeResult = ArgumentParser.GetSize(size.Value());
                        if (sizeResult.IsSuccess)
                            _size = sizeResult.Value;
                        else
                            ErrorHandler.ThrowError(sizeResult.Error);
                    }

                    return 0;
                }
            );
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IWordFilter>(new PartsOfSpeechFilter(PartsOfSpeech.ADVPRO, PartsOfSpeech.APRO,
                PartsOfSpeech.CONJ));
            var coloringAlgorithm = new DefaultColoringAlgorithm(_textColor);
            services.AddSingleton<ILayoutAlgorithm>(
                new CircularCloudLayouter(new Point(_size.Width / 2, _size.Height / 2)));
            services.AddSingleton<IColoringAlgorithm>(coloringAlgorithm);
            services.AddSingleton<IImageConfig>(new ImageConfig.ImageConfig(_size, _fontFamily, _backgroundColor,
                coloringAlgorithm));
            services.AddSingleton<IPointGenerator, ArchimedeanSpiral>();
            services.AddSingleton(ChooseReader());
            services.AddSingleton<IBitmapCreator, BitmapCreator.BitmapCreator>();
            _serviceProvider = services.BuildServiceProvider();
        }

        private static IFileReader ChooseReader()
        {
            if (_pathToInputFile.EndsWith(".txt"))
                return new TxtFileReader();

            if (_pathToInputFile.EndsWith(".docx"))
                return new DocxFileReader();

            throw new ArgumentException("Unsupported file format");
        }
    }
}

