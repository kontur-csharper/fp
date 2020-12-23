using System.Collections.Generic;

namespace TagsCloudContainer
{
    public interface IWordRenderer
    {
        IEnumerable<LayoutedWord> SizeWords(IEnumerable<LayoutedWord> words);
        Result<None> Render(IEnumerable<LayoutedWord> words);
    }
}