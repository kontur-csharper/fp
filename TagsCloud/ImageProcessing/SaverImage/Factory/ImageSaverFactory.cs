using System.IO;
using System.Linq;
using TagsCloud.Factory;
using TagsCloud.ImageProcessing.Config;
using TagsCloud.ImageProcessing.SaverImage.ImageSavers;
using TagsCloud.ResultOf;

namespace TagsCloud.ImageProcessing.SaverImage.Factory
{
    public class ImageSaverFactory : ServiceFactory<IImageSaver>
    {
        private readonly IImageConfig imageConfig;

        public ImageSaverFactory(IImageConfig imageConfig)
        {
            this.imageConfig = imageConfig;
        }

        public override Result<IImageSaver> Create()
        {
            var saver = services.FirstOrDefault(pair => pair.Value().CanSave(imageConfig.Path)).Value;

            return Result.Of(saver, $"This image type {Path.GetExtension(imageConfig.Path)} is not supported");
        }
    }
}
