using System.Collections.Generic;
using System.Linq;

namespace TagsCloud.Settings.SettingsForTextProcessing
{
    public class TextProcessingSettings : ITextProcessingSettings
    {
        public HashSet<string> BoringWords { get; }

        public TextProcessingSettings(string[] boringWords)
        {
            BoringWords = boringWords.ToHashSet();
        }
    }
}