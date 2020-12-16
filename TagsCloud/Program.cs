using System;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using RectanglesCloudLayouter.CalculatorForCloudRadius;
using RectanglesCloudLayouter.LayouterOfRectangles;
using RectanglesCloudLayouter.SettingsForSpiral;
using RectanglesCloudLayouter.SpiralAlgorithm;
using ResultPattern;
using TagsCloud.Clients;
using TagsCloud.ProcessorOfApp;
using TagsCloud.Reader;
using TagsCloud.Saver;
using TagsCloud.Settings.SettingsForStorage;
using TagsCloud.Settings.SettingsForTextProcessing;
using TagsCloud.Settings.SettingsForVisualization;
using TagsCloud.Settings.TemporaryStorageForSettings;
using TagsCloud.TagsCloudVisualization;
using TagsCloud.TagsLayouter;
using TagsCloud.TextProcessing.FrequencyOfWords;
using TagsCloud.TextProcessing.ParserForWordsAndSpeechParts;
using TagsCloud.TextProcessing.TextConverters;
using TagsCloud.TextProcessing.TextFilters;
using TagsCloud.TextProcessing.WordsMeasurer;

namespace TagsCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ConsoleClient();
            if (!client.TryGetUserCommands(args, out var commands))
                return;

            TemporarySettingsStorage
                .From(commands)
                .Then(settings =>
                {
                    var serviceCollection = new ServiceCollection();
                    InjectDependencies(serviceCollection, settings);
                    return serviceCollection;
                })
                .Then(serviceCollection => serviceCollection
                    .BuildServiceProvider()
                    .GetRequiredService<IAppProcessor>()
                    .Run())
                .OnFail(Console.WriteLine);
        }

        private static void InjectDependencies(IServiceCollection serviceCollection,
            TemporarySettingsStorage settingsStorage)
        {
            serviceCollection.AddSingleton<IAppProcessor, AppProcessor>()
                .AddSingleton<IFileReader, FileReader>()
                .AddSingleton<IImageSaver, ImageSaver>()
                .AddSingleton<IStorageSettings>(new StorageSettings(settingsStorage.PathToCustomText,
                    settingsStorage.PathToSave,
                    settingsStorage.ImageFormat))
                .AddSingleton<IVisualizationSettings>(new VisualizationSettings(settingsStorage.ImageSize,
                    settingsStorage.BackgroundColor,
                    settingsStorage.TextColor, settingsStorage.Font))
                .AddSingleton<ITextProcessingSettings>(new TextProcessingSettings(settingsStorage.BoringWords))
                .AddSingleton<IVisualization, Visualization>()
                .AddTransient<IRectanglesLayouter, RectanglesLayouter>()
                .AddSingleton<ISpiral>(new ArchimedeanSpiral(new Point(0, 0),
                    new SpiralSettings(settingsStorage.AdditionSpiralAngleFromDegrees, settingsStorage.SpiralStep)))
                .AddTransient<ICloudRadiusCalculator, CloudRadiusCalculator>()
                .AddTransient<IWordTagsLayouter, WordTagsLayouter>()
                .AddSingleton(settingsStorage.Font)
                .AddSingleton<IWordsFrequency, WordsFrequency>()
                .AddSingleton<IWordMeasurer, WordMeasurer>()
                .AddSingleton<IWordsFilter, WordsFilter>()
                .AddSingleton<IWordsFilter, SpeechPartsFilter>()
                .AddSingleton(new[] {"PR", "PART", "INTJ", "CONJ", "ADVPRO", "APRO", "NUM", "SPRO"})
                .AddSingleton<INormalizedWordAndSpeechPartParser, NormalizedWordAndSpeechPartParser>()
                .AddSingleton<ITextConverter>(new MyStemConverter(Path.GetFullPath("mystem.exe"), "-ni"));
        }
    }
}