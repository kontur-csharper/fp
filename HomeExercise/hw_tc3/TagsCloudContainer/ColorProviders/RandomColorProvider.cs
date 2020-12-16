using System;
using System.Drawing;

namespace TagsCloudContainer
{
    class RandomColorProvider : IColorProvider
    {
        private Random random;
        public RandomColorProvider()
        {
            random = new Random();
        }
        public Result<Color> GetNextColor()
        {
            var r = random.Next(0, 255);
            var g = random.Next(0, 255);
            var b = random.Next(0, 255);
            return Result.Of(() => Color.FromArgb(r, g, b));
        }
    }
}
