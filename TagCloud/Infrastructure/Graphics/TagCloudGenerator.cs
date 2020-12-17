using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Infrastructure.Text;
using TagCloud.Infrastructure.Text.Conveyors;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Graphics
{
    public class TagCloudGenerator : IImageGenerator
    {
        private readonly IEnumerable<IConveyor> conveyors;
        private readonly IPainter<string> painter;
        private readonly IReader<string> reader;

        public TagCloudGenerator(IReader<string> reader, IEnumerable<IConveyor> conveyors,
            IPainter<string> painter)
        {
            this.reader = reader;
            this.conveyors = conveyors;
            this.painter = painter;
        }

        public Result<Image> Generate()
        {
            var tokens = reader.ReadTokens();
            var analyzedTokens = conveyors.Aggregate(
                tokens.Select(line => new TokenInfo(line)),
                (current, filter) => filter.Handle(current).ToArray());
            return painter.GetImage(analyzedTokens);
        }
    }
}