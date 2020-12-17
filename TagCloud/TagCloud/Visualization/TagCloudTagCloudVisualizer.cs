using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using TagCloud.ErrorHandling;

namespace TagCloud.Visualization
{
    public class TagCloudTagCloudVisualizer : ITagCloudVisualizer
    {
        private readonly ITagCloud tagCloud;

        public TagCloudTagCloudVisualizer(ITagCloud tagCloud)
        {
            this.tagCloud = tagCloud;
        }

        public Result<Bitmap> CreateTagCloudBitMap(int width, int height, Color[] colors, string fontFamily)
        {
            if (tagCloud.WordRectangles.Count == 0)
                return new Bitmap(width, height);
            if (!CheckCorrectnessOfInput(width, height, colors, fontFamily, out var error))
                return Result.Fail<Bitmap>(error);
            var bitMap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitMap);
            graphics.Clear(Color.Black);
            using var cycledColors = colors.Cycle().GetEnumerator();
            cycledColors.MoveNext();
            foreach (var wordRectangle in tagCloud.WordRectangles)
            {
                var color = cycledColors.Current;
                var word = wordRectangle.Word;
                var rectangle = wordRectangle.Rectangle;
                var font = GetBiggestFont(wordRectangle, fontFamily, graphics);
                graphics.DrawString(word, font, new SolidBrush(color), new PointF(rectangle.Left, rectangle.Y));
                graphics.DrawPolygon(new Pen(color), RectangleToPointFArray(rectangle));

                cycledColors.MoveNext();
            }

            return bitMap;
        }

        private bool CheckCorrectnessOfInput(int width, int height, Color[] colors, string fontFamily, out string error)
        {
            error = null;
            var firstRectangle = tagCloud.WordRectangles[0].Rectangle;
            var screen = new Rectangle(0, 0, width, height);
            if (!CanTagCloudFitTheScreen(screen))
            {
                var tagCloudSize = CoveredArea().Size;
                error = "Tag cloud is too large for that resolution. Tag cloud size is " +
                        $"{tagCloudSize.Width}x{tagCloudSize.Height} and " +
                        $"center is {firstRectangle.Location.X + firstRectangle.Width / 2}," +
                        $"{firstRectangle.Location.Y + firstRectangle.Height / 2}";
                return false;
            }

            var installedFonts = new InstalledFontCollection().Families.Select(family => family.Name);
            if (!installedFonts.Contains(fontFamily))
            {
                error = $"Font {fontFamily} is not supported";
                return false;
            }

            if (colors == null || colors.Length == 0)
            {
                error = "Colors are not specified";
                return false;
            }

            return true;
        }

        private bool CanTagCloudFitTheScreen(Rectangle screen)
        {
            return screen.Contains(CoveredArea());
        }

        private Rectangle CoveredArea()
        {
            var maxX = tagCloud.WordRectangles.Max(rectangle => rectangle.Rectangle.Right);
            var maxY = tagCloud.WordRectangles.Max(rectangle => rectangle.Rectangle.Top);
            var minX = tagCloud.WordRectangles.Min(rectangle => rectangle.Rectangle.Left);
            var minY = tagCloud.WordRectangles.Min(rectangle => rectangle.Rectangle.Bottom);
            var width = maxX - minX;
            var height = maxY - minY;

            return new Rectangle(minX, minY, width, height);
        }

        private Font GetBiggestFont(WordRectangle token, string fontFamilyName, Graphics graphics)
        {
            var word = token.Word;
            var rectangle = token.Rectangle;
            var fontSize = 10;
            for (; fontSize < 100; fontSize++)
            {
                var font = new Font(fontFamilyName, fontSize);
                var size = graphics.MeasureString(word, font);
                if (size.Height > rectangle.Height || size.Width > rectangle.Width)
                    break;
            }

            return new Font(fontFamilyName, fontSize - 1);
        }

        private static PointF[] RectangleToPointFArray(Rectangle rectangle)
        {
            return new[]
            {
                new PointF(rectangle.Left, rectangle.Bottom),
                new PointF(rectangle.Right, rectangle.Bottom),
                new PointF(rectangle.Right, rectangle.Top),
                new PointF(rectangle.Left, rectangle.Top)
            };
        }
    }
}