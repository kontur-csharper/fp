using System.Collections.Generic;
using System.Drawing;
using TagsCloud.WordLayouters;

namespace TagsCloud.CloudRenderers
{
    public class CloudRenderer : ICloudRenderer
    {
        private readonly IWordLayouter layouter;
        private readonly int width;
        private readonly int height;
        private readonly string filePath;

        public CloudRenderer(IWordLayouter layouter, int width, int height, string filePath)
        {
            this.layouter = layouter;
            this.width = width;
            this.height = height;
            this.filePath = filePath;
        }
        
        public Result<string> RenderCloud()
        {
            if (width <= 0 || height <= 0) return Result.Fail<string>("Not positive width or height");
            
            var wordsRectangle = layouter.GetCloudRectangle();
            var words = layouter.CloudWords;

            return RenderCloud(wordsRectangle, words).RefineError("Failed to render image");
        }

        private Result<string> RenderCloud(Rectangle wordsRectangle, IReadOnlyCollection<CloudWord> words) =>
            Result.Of(() => new Bitmap(width, height))
                .RefineError($"Failed to create image with width={width}, height={height}")
                .Then(bitmap =>
                {
                    using (bitmap)
                    {
                        using var graphics = Graphics.FromImage(bitmap);
                        graphics.ScaleTransform((float) width / wordsRectangle.Width,
                            (float) height / wordsRectangle.Height);
                        graphics.TranslateTransform(-wordsRectangle.X, -wordsRectangle.Y);
                        foreach (var word in words)
                            graphics.DrawString(word.Value, word.Font, new SolidBrush(word.Color),
                                word.Rectangle.Location);

                        return Result.OfAction(() => bitmap.Save(filePath))
                            .RefineError($"Failed to save image to path = `{filePath}`")
                            .Then(_ => filePath);
                    }
                });
    }
}