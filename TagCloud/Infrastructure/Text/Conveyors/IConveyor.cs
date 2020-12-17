using System.Collections.Generic;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public interface IConveyor
    {
        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens);
    }
}