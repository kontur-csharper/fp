using System.Drawing;
using ResultOf;
using TagCloud.Settings;
using TagCloud.TagClouds;
using TagCloud.Visualizers;

namespace TagCloud.Renderers
{
    public class FileCloudRender : IRender
    {
        private readonly IVisualizer<RectangleTagCloud> visualizer;
        private readonly ResultSettings settings;

        public FileCloudRender(IVisualizer<RectangleTagCloud> visualizer, ResultSettings settings)
        {
            this.visualizer = visualizer;
            this.settings = settings;
        }

        public Result<None> Render()
        {
            var leftUpBound = visualizer.VisualizeTarget.LeftUpBound;
            var rightDownBound = visualizer.VisualizeTarget.RightDownBound;

            return CreateImage(leftUpBound, rightDownBound)
                .Then(Visualize)
                .Then(image => image.Save($"{settings.FileName}.png"));
        }

        private Result<Bitmap> CreateImage(Point leftUp, Point rightDown)
        {
            return Result.Of(() => new Bitmap(
                rightDown.X - leftUp.X,
                rightDown.Y - leftUp.Y));
        }

        private Result<Bitmap> Visualize(Bitmap image)
        {
            return Result.Of(() => Graphics.FromImage(image))
                .Then(graphic => visualizer.Draw(graphic))
                .Then(none => image);
        }
    }
}
