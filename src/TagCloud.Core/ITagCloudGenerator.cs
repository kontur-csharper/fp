using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using FunctionalStuff.Results;
using TagCloud.Core.Layouting;
using TagCloud.Core.Text.Formatting;

namespace TagCloud.Core
{
    public interface ITagCloudGenerator : IDisposable
    {
        Task<Result<Image>> DrawWordsAsync(
            FontSizeSourceType sizeSourceType,
            LayouterType layouterType,
            Color[] palette,
            Dictionary<string, int> words,
            FontFamily fontFamily,
            Point centerPoint,
            Size distance,
            CancellationToken token);
    }
}