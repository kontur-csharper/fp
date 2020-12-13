namespace TagsCloudContainer
{
    public interface IFontDetailsCreator
    {
        string GetFontName(int wordsCount, int maxWordsCount);
        float GetFontSize(int wordsCount, int maxWordsCount);
    }
}