using System.Drawing;
using System.Drawing.Imaging;
using ResultPattern;
using TagsCloud.ConvertersAndCheckersForSettings.CheckerForDirectory;
using TagsCloud.ConvertersAndCheckersForSettings.CheckerForFile;
using TagsCloud.ConvertersAndCheckersForSettings.ConverterForFont;
using TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageFormat;
using TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageSize;
using TagsCloud.UserCommands;
using FontConverter = TagsCloud.ConvertersAndCheckersForSettings.ConverterForFont.FontConverter;
using ImageFormatConverter = TagsCloud.ConvertersAndCheckersForSettings.ConverterForImageFormat.ImageFormatConverter;

namespace TagsCloud.Settings.TemporaryStorageForSettings
{
    public class TemporarySettingsStorage
    {
        private static readonly IDirectoryChecker _directoryChecker = new DirectoryExistsChecker();
        private static readonly IFileExistsСhecker _fileExistsСhecker = new FileExistsСhecker();
        private static readonly IFontConverter _fontConverter = new FontConverter();
        private static readonly IImageFormatConverter _imageFormatConverter = new ImageFormatConverter();
        private static readonly IImageSizeConverter _imageSizeConverter = new ImageSizeConverter();

        public string PathToCustomText { get; set; }

        public string PathToSave { get; set; }

        public ImageFormat ImageFormat { get; set; }

        public Font Font { get; set; }

        public Size ImageSize { get; set; }

        public Color TextColor { get; set; }

        public Color BackgroundColor { get; set; }

        public string[] BoringWords { get; set; }

        public double AdditionSpiralAngleFromDegrees { get; set; }

        public double SpiralStep { get; set; }

        public static Result<TemporarySettingsStorage> From(AllUserCommands commands)
        {
            return new TemporarySettingsStorage()
                .AsResult()
                .Then(storage =>
                {
                    var result = _fileExistsСhecker.GetProvenPath(commands.PathToCustomText);
                    if (!result.IsSuccess) return ResultExtensions.Fail<TemporarySettingsStorage>(result.Error);
                    storage.PathToCustomText = result.GetValueOrThrow();
                    return ResultExtensions.Ok(storage);
                }).Then(storage =>
                {
                    var result = _directoryChecker.GetExistingDirectory(commands.PathToSave);
                    if (!result.IsSuccess) return ResultExtensions.Fail<TemporarySettingsStorage>(result.Error);
                    storage.PathToSave = result.GetValueOrThrow();
                    return ResultExtensions.Ok(storage);
                }).Then(storage =>
                {
                    var result = _imageFormatConverter.ConvertToImageFormat(commands.ImageFormat);
                    if (!result.IsSuccess) return ResultExtensions.Fail<TemporarySettingsStorage>(result.Error);
                    storage.ImageFormat = result.GetValueOrThrow();
                    return ResultExtensions.Ok(storage);
                }).Then(storage =>
                {
                    var result = _imageSizeConverter.ConvertToSize(commands.ImageSize);
                    if (!result.IsSuccess) return ResultExtensions.Fail<TemporarySettingsStorage>(result.Error);
                    storage.ImageSize = result.GetValueOrThrow();
                    return ResultExtensions.Ok(storage);
                })
                .Then(storage =>
                {
                    var result = _fontConverter.ConvertToFont(commands.Font);
                    if (!result.IsSuccess) return ResultExtensions.Fail<TemporarySettingsStorage>(result.Error);
                    storage.Font = result.GetValueOrThrow();
                    return ResultExtensions.Ok(storage);
                })
                .Then(storage =>
                {
                    storage.BackgroundColor = Color.FromKnownColor(commands.BackgroundColor);
                    storage.TextColor = Color.FromKnownColor(commands.TextColor);
                    storage.BoringWords = commands.BoringWords;
                    storage.AdditionSpiralAngleFromDegrees = commands.AdditionSpiralAngleFromDegrees != 0
                        ? commands.AdditionSpiralAngleFromDegrees
                        : 1;
                    storage.SpiralStep = commands.SpiralStep != 0 ? commands.SpiralStep : 0.5;
                    return ResultExtensions.Ok(storage);
                });
        }
    }
}