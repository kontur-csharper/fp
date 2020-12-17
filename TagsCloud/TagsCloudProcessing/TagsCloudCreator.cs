using System.Drawing;
using TagsCloud.Factory;
using TagsCloud.ImageProcessing.Config;
using TagsCloud.ImageProcessing.ImageBuilders;
using TagsCloud.ImageProcessing.SaverImage.ImageSavers;
using TagsCloud.ResultOf;
using TagsCloud.TagsCloudProcessing.TagsGenerators;
using TagsCloud.TextProcessing;

namespace TagsCloud.TagsCloudProcessing
{
    public class TagsCloudCreator
    {
        private readonly TextProcessor textProcessor;
        private readonly IImageBuilder imageBuilder;
        private readonly IServiceFactory<IImageSaver> imageSaverFactory;
        private readonly IServiceFactory<ITagsGenerator> tagsGeneratorFactory;
        private readonly IImageConfig imageConfig;

        public TagsCloudCreator(TextProcessor textProcessor,
             IImageBuilder imageBuilder,
             IServiceFactory<IImageSaver> imageSaverFactory,
             IServiceFactory<ITagsGenerator> tagsGeneratorFactory,
             IImageConfig imageConfig)
        {
            this.tagsGeneratorFactory = tagsGeneratorFactory;
            this.textProcessor = textProcessor;
            this.imageSaverFactory = imageSaverFactory;
            this.imageBuilder = imageBuilder;
            this.imageConfig = imageConfig;
        }

        public Result<None> CreateCloud(string textPath, string imageSavePath)
        {
            using var image = new Bitmap(imageConfig.ImageSize.Width, imageConfig.ImageSize.Height);
            var wordsFromFile = textProcessor.ReadFromFile(textPath);
            var tags = tagsGeneratorFactory.Create()
                .Then(generator => wordsFromFile.Then(words => generator.CreateTags(words)));

            var imageResult = tags.Then(tags => imageBuilder.DrawTags(tags, image));
            return imageSaverFactory.Create()
                .Then(saver => imageResult.Then(imgRes => saver.SaveImage(image, imageSavePath)));
        }
    }
}
