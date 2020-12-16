using System;
using System.Drawing;
using ResultPattern;
using TagsCloud.Settings.SettingsForStorage;

namespace TagsCloud.Saver
{
    public class ImageSaver : IImageSaver
    {
        private readonly IStorageSettings _storageSettings;

        public ImageSaver(IStorageSettings storageSettings)
        {
            _storageSettings = storageSettings;
        }

        public Result<None> Save(Image image)
        {
            try
            {
                var pathToSave = $"{_storageSettings.PathToSave}.{_storageSettings.ImageFormat.ToString().ToLower()}";
                image.Save(pathToSave,
                    _storageSettings.ImageFormat);
                Console.WriteLine($@"The image was saved in the specified path: {pathToSave}");
                return new Result<None>();
            }
            catch (Exception)
            {
                return new Result<None>("Failed to save image");
            }
        }
    }
}