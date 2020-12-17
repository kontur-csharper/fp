using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class OrderConveyor : IConveyor
    {
        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens)
        {
            return tokens.OrderByDescending(x => x.Frequency);
        }
    }
}