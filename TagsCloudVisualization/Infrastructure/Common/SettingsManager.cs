using TagsCloudCreating.Configuration;

namespace TagsCloudVisualization.Infrastructure.Common
{
    public class SettingsManager
    {
        public ImageSettings ImageSettings { get; set; } = new ImageSettings();
        public CloudLayouterSettings LayouterSettings { get; set; } = new CloudLayouterSettings();
        public TagsSettings TagsSettings { get; set; } = new TagsSettings();
        public WordHandlerSettings WordHandlerSettings { get; set; } = new WordHandlerSettings();
    }
}