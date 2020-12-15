using TagsCloudContainer.Infrastructure.CloudGenerator;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.App.Settings
{
    public class LayouterAlgorithmSettings : ILayouterAlgorithmSettingsHolder
    {
        public static readonly CloudLayouterAlgorithm DefaultAlgorithm = CloudLayouterAlgorithm.CircularCloudLayouter;

        public static readonly LayouterAlgorithmSettings Instance = new LayouterAlgorithmSettings();

        private LayouterAlgorithmSettings()
        {
            SetDefault();
        }

        public CloudLayouterAlgorithm LayouterAlgorithm { get; set; }

        public void SetDefault()
        {
            LayouterAlgorithm = DefaultAlgorithm;
        }
    }
}