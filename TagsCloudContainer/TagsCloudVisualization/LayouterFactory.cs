using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagsCloudContainer.TagsCloudVisualization.Interfaces;

namespace TagsCloudContainer.TagsCloudVisualization
{
    public class LayouterFactory : ILayouterFactory
    {
        private readonly IEnumerable<ISpiral> spirals;

        public LayouterFactory(IEnumerable<ISpiral> spirals)
        {
            this.spirals = spirals;
        }

        public ILayouter GetLayouter(SpiralType type)
        {
            return Result.Ok(type)
                .Then(ValidateSpiralType)
                .OnFail(e => throw new ArgumentException(e, nameof(type)))
                .SelectMany(x => Result.Ok(new CloudLayouter(spirals.First(y => y.Type == type))))
                .GetValueOrThrow();
        }

        private Result<SpiralType> ValidateSpiralType(SpiralType type)
        {
            return spirals.Select(x => x.Type).Any(x => x == type)
                ? Result.Ok(type)
                : Result.Fail<SpiralType>($"Factory doest contain spiral type: {type}");
        }
    }
}