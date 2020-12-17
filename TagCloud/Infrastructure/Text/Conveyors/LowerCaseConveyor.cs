using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class LowerCaseConveyor : IConveyor
    {
        public IEnumerable<TokenInfo> Handle(IEnumerable<TokenInfo> tokens)
        {
            return tokens.Select(pair =>
            {
                pair.Token = pair.Token.ToLower();
                return pair;
            });
        }
    }
}