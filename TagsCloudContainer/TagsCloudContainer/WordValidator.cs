using TagsCloudContainer.TagsCloudContainer.Interfaces;

namespace TagsCloudContainer.TagsCloudContainer
{
    public class WordValidator : IWordValidator
    {
        private const int MinWordLength = 2;

        public bool IsValidWord(string word)
        {
            return word.Length > MinWordLength;
        }
    }
}