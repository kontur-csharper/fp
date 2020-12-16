using System.Collections.Generic;

namespace TagsCloud.Settings.SettingsForTextProcessing
{
    public interface ITextProcessingSettings
    {
        HashSet<string> BoringWords { get; }
    }
}