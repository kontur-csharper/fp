using System.Collections.Generic;
using System.IO;
using CommandLine;
using TagCloud.ExceptionHandler;
using TagCloud.ImageSaver;

namespace TagCloud.Visualizer.Console
{
    [Verb("print")]
    public class PrintCommand
    {
        private static readonly string SavePath = Path.Combine(Directory.GetCurrentDirectory(),
            "..",
            "..",
            "..",
            "..",
            $"{nameof(TagCloud)}.{nameof(Visualizer)}",
            "images");

        public int PrintCloudAndReturnExitCode(ICloudLayouter cloudLayouter, Result<IEnumerable<string>> wordsResult,
            ImageOptions imageOptions, IImageSaver imageSaver)
        {
            var printResult = wordsResult
                .Then(words => SizesCreator.CreateSizesArray(words, imageOptions.FontSize, imageOptions.FontName))
                .Then(sizes => cloudLayouter.GetRectangles(sizes))
                .Then(rects => BitmapCreator.DrawBitmap(rects, imageOptions))
                .Then(bitmap => imageSaver.SaveImage(bitmap, SavePath, imageOptions));
            if (printResult.IsSuccess)
            {
                return 0;
            }

            System.Console.WriteLine(printResult.Error);
            return -1;
        }
    }
}