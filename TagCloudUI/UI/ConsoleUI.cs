using System;
using TagCloud.Core;
using TagCloud.Core.ImageSavers;
using TagCloudUI.Infrastructure;

namespace TagCloudUI.UI
{
    public class ConsoleUI
    {
        private readonly ITagCloudHandler tagCloudHandler;
        private readonly IAppSettings settings;
        private readonly IImageSaver imageSaver;

        public ConsoleUI(
            ITagCloudHandler tagCloudHandler,
            IAppSettings settings,
            IImageSaver imageSaver)
        {
            this.tagCloudHandler = tagCloudHandler;
            this.settings = settings;
            this.imageSaver = imageSaver;
        }

        public void Run()
        {
            tagCloudHandler.Run(settings)
                .Then(bitmap => imageSaver.Save(bitmap, settings.OutputPath, settings.ImageFormat))
                .Then(savedPath => Console.WriteLine($"Tag cloud visualization saved to: {savedPath}"))
                .OnFail(PrintErrorAndExit);
        }

        private static void PrintErrorAndExit(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }
    }
}