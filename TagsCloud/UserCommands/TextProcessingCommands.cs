using MatthiWare.CommandLine.Core.Attributes;

namespace TagsCloud.UserCommands
{
    public class TextProcessingCommands : StorageCommands
    {
        [Required, Name("w", "boring"), Description("Boring words, space separated between words")]
        public string[] BoringWords { get; set; }
    }
}