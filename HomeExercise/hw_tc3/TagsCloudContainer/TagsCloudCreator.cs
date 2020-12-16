using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudContainer
{
    public class TagsCloudCreator
    {
        private FontFamily fontFamily;
        private readonly ICloudDrawer cloudDrawer;
        private readonly IFontSizeCalculator fontSizeCalculator;
        private readonly Dictionary<string, IFileReader> fileReaders;
        private readonly Dictionary<string, IImageSaver> imageSavers;
        private readonly List<IWordsFilter> wordsFilters;
        private StopWords stopWords;

        public TagsCloudCreator(
            ICloudDrawer cloudDrawer, 
            IFontSizeCalculator fontSizeCalculator, 
            IFileReader[] fileReaders, 
            IImageSaver[] imageSavers, 
            IWordsFilter[] wordsFilters,
            StopWordsFilter stopWordsFilter)
        {
            this.cloudDrawer = cloudDrawer;
            this.fontSizeCalculator = fontSizeCalculator;
            this.fileReaders = InitializeFileReaders(fileReaders);
            this.imageSavers = InitializeImageSavers(imageSavers);
            fontFamily = new FontFamily("Arial");
            stopWords = stopWordsFilter.StopWords;
            this.wordsFilters = new List<IWordsFilter> { stopWordsFilter };
            this.wordsFilters.AddRange(wordsFilters);
        }

        private Dictionary<string, IFileReader> InitializeFileReaders(IFileReader[] fileReaders)
        {
            return fileReaders.ToDictionary(fr => fr.Format, fr => fr);
        }

        private Dictionary<string, IImageSaver> InitializeImageSavers(IImageSaver[] imageSavers)
        {
            return imageSavers.ToDictionary(ims => ims.FormatName, ims => ims);
        }

        public Result<None> Create(string filePath, string targetPath, string imageName)
        {
            return Result.AsResult(Path.GetExtension(filePath).TrimStart('.'))
                .RefineError("Некорректный путь до исходного файла")
                .Then(x => fileReaders[x])
                .RefineError("Данный тип текстового файла не поддерживается")
                .Then(reader => reader.ReadAllLines(filePath).Value.ToList())
                .Then(FilterWords)
                .Then(words => fontSizeCalculator.CalculateFontSize(words, fontFamily))
                .Then(sizedWords => sizedWords.OrderByDescending(word => word.Font.Size).ToList())
                .Then(sortedWords => cloudDrawer.DrawCloud(sortedWords, targetPath, imageName));
        }

        public Result<None> AddStopWord(string stopWord)
        {
            return Result.OfAction(() => stopWords.Add(stopWord),
                $"Возникла ошибка при удалении стопслова {stopWord}");
        }

        public Result<None> RemoveStopWord(string stopWord)
        {
            return Result.OfAction(() => stopWords.Remove(stopWord),
                $"Возникла ошибка при удалении стопслова {stopWord}");
        }

        public Result<None> SetFontFamily(string fontFamilyName)
        {
            return Result.OfAction(() => fontFamily = new FontFamily(fontFamilyName),
                $"Шрифт с именем {fontFamilyName} не найден в системе");
        }

        public Result<None> SetFontColor(string colorName)
        {
            var color = Color.FromName(colorName);
            if (color.R == 0 && color.G == 0 && color.B == 0 && color.Name != "Black")
                return Result.Fail<None>($"Цвет {colorName} не поддерживается");
            cloudDrawer.ColorProvider = new FixedColorProvider(color);
            return Result.Ok();
        }

        public Result<None> SetFontRandomColor()
        {
            return Result.OfAction(() => cloudDrawer.ColorProvider = new RandomColorProvider(),
                "Ошибка при установке рандомного цвета");
        }

        public Result<None> SetImageSize(int imageSize)
        {
            if (imageSize > 2000 || imageSize < 100)
                return Result.Fail<None>("Неверный размер изображения \n(не больше 2000px и не меньше 100px)");
            return Result.OfAction(() => cloudDrawer.ChangeImageSize(imageSize));
        }

        public Result<None> SetImageFormat(string imageFormat)
        {
            return Result.OfAction(() => cloudDrawer.ImageSaver = imageSavers[imageFormat.ToLower()],
                $"Фрмат изображения {imageFormat.ToLower()} не поддерживается");
        }

        public string GetImageFormat()
        {
            return cloudDrawer.ImageSaver.FormatName;
        }

        private Result<List<string>> FilterWords(List<string> words)
        {
            return wordsFilters.Aggregate(Result.Ok(words), 
                (notFilteredWords, nextFilter) => notFilteredWords.Then(words => nextFilter.Filter(words)))
                .RefineError("Произошла ошибка при фильтрации слов");
        }
    }
}
