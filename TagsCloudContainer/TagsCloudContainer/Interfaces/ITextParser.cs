using System.Collections.Generic;

namespace TagsCloudContainer.TagsCloudContainer.Interfaces
{
    public interface ITextParser
    {
        List<string> GetAllWords(string text);
    }
}