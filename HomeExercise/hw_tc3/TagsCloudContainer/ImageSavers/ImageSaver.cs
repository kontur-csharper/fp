using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer
{
    abstract class ImageSaver : IImageSaver
    {
        public string FormatName { get; set; }
        public ImageFormat Format { get; set; }

        public Result<None> Save(string path, string name, Bitmap bitmap)
        {
            return Result.OfAction(() => bitmap.Save($"{path}\\{name}.{FormatName}", Format), 
                "Не удалось сохранить файл с изображением");
        }
    }
}
