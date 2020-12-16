namespace TagsCloud.TextProcessing.TextConverters
{
    public interface ITextConverter
    {
        string ConvertTextToCertainFormat(string originalText);
    }
}