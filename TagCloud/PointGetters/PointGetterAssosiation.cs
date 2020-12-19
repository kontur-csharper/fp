using System.Collections.Generic;
using System.Linq;
using System;
using ResultOf;

namespace TagCloud.PointGetters
{
    public static class PointGetterAssosiation
    {
        public const string circle = "circle";
        public const string spiral = "spiral";
        private static readonly Dictionary<string, IPointGetter> pointGetters =
            new Dictionary<string, IPointGetter>
            {
                [circle] = new CirclePointGetter(),
                [spiral] = new SpiralPointGetter()
            };
        public static readonly HashSet<string> getters = pointGetters.Keys.ToHashSet();

        public static Result<IPointGetter> GetPointGetter(string name)
        {
            if (!pointGetters.ContainsKey(name))
            {
                return new Result<IPointGetter>($"doesn't have point getter with name {name}\n" +
                    $"List of text point getter names:\n{string.Join('\n', getters)}");
            }
            return new Result<IPointGetter>(null, pointGetters[name]);
        }
    }
}
