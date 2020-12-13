using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using LightInject;
using ResultOf;
using TagsCloudVisualization;
using WeCantSpell.Hunspell;

namespace TagsCloudContainer
{
    class Program
    {
        private static readonly string _pathToDict = "Dictionaries/English (American).dic";
        private static readonly string _pathToAffixFile = "Dictionaries/English (American).aff";
        private static readonly string[] _supportedTextFormats = { ".txt", ".docx" };

        static void Main(string[] args)
        {
            ParseArguments(args)
                .Then(ValidateSettings)
                .Then(DrawImage)
                .OnFail(Console.WriteLine);
        }

        private static Result<AppSettings> ParseArguments(string[] args)
        {
            return Result
                .Of(() => Parser.Default.ParseArguments<AppSettings>(args))
                .Validate(
                    settings => settings.Tag == ParserResultType.Parsed,
                    "Incorrect commandline arguments")
                .Then(settings => ((Parsed<AppSettings>) settings).Value);
        }

        private static AppSettings ValidateSettings(AppSettings settings)
        {
            return new AppSettingsValidator(settings)
                .ValidateTextFilePath(_supportedTextFormats)
                .ValidatePartsOfSpeech()
                .ValidateColors()
                .ValidateFont()
                .ValidateImagePath(ImageSaver.SupportedFormats)
                .Settings;
        }

        private static Result<None> DrawImage(AppSettings settings)
        {
            return Result.Of(() => GetWordsLoader(settings.PathToFile))
                .Then(loader => loader.GetWords())
                .Then(words => 
                    GetWordsCounter(settings.ExcludedPartsOfSpeechNames).CountWords(words))
                .Then(countedWords =>
                    GetDrawer(settings).Draw(countedWords))
                .Then(image => SaveImage(image, settings.ImagePath));
        }

        private static IWordsLoader GetWordsLoader(string pathToFile)
        {
            return Path.GetExtension(pathToFile) switch
            {
                ".docx" => new DocxFileWordsLoader(pathToFile),
                ".txt" => new TxtFileWordsLoader(pathToFile),
            };
        }

        private static IWordsCounter GetWordsCounter(IEnumerable<string> excludedPartsOfSpeechNames)
        {
            var excludedPartsOfSpeech = excludedPartsOfSpeechNames.Select(
                partOfSpeech => Enum.Parse<PartOfSpeech>(partOfSpeech, true));

            var appDirectory = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);
            var wordList = WordList.CreateFromFiles( 
                Path.GetFullPath(_pathToDict, appDirectory),
                Path.GetFullPath(_pathToAffixFile, appDirectory));

            return new MorphologicalWordsCounter(wordList, excludedPartsOfSpeech);
        }

        private static ITagsCloudDrawer GetDrawer(AppSettings appSettings)
        {
            var container = new ServiceContainer();

            container.Register<IFontColorCreator>(factory =>
                new FontColorCreator(appSettings.FontColor));

            container.Register<IFontDetailsCreator>(factory =>
                new FontDetailsCreator(appSettings.FontName));
            
            var center = new Point(appSettings.ImageWidth/2, appSettings.ImageHeight/2);
            var angleInRadians = appSettings.AngleStepInDegrees * Math.PI / 180;
            container.Register<ICloudLayouter>(factory =>
                new CircularCloudLayouter(center, new Spiral(angleInRadians, appSettings.ShiftFactor)));
            
            container.Register<IDrawSettings>(factory => appSettings);
            
            container.Register<ITagsCloudDrawer, TagsCloudDrawer>();

            return container.GetInstance<ITagsCloudDrawer>();
        }

        private static void SaveImage(Bitmap image, string imagePath)
        {
            new ImageSaver(imagePath).Save(image);
        }
    }
}