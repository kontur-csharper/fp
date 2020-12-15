namespace TagsCloudContainer.TagsCloudContainer.Interfaces
{
    public interface ITextWriter
    {
        void WriteText(string text, ITextSaver saver);
    }
}